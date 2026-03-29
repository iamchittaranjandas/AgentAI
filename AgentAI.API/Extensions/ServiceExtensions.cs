using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Threading.RateLimiting;

namespace AgentAI.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // JWT Authentication
        var jwtKey = configuration["Jwt:Key"]!;
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Unauthorized. Please provide a valid Bearer token.",
                        errors = new[] { context.ErrorDescription ?? "Authentication required." }
                    });
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Forbidden. You do not have permission to access this resource.",
                        errors = new[] { "Insufficient role." }
                    });
                }
            };
        });

        services.AddAuthorization();

        // Swagger with JWT Bearer support
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AgentAI API",
                Version = "v1",
                Description = "Enterprise-Grade AI Developer Assistant API"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token. Example: Bearer eyJhbGci..."
            });

            options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", null),
                    new List<string>()
                }
            });
        });

        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);

        // API Versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddMvc();

        // Rate Limiting — keyed by authenticated user ID or IP
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                             ?? context.Connection.RemoteIpAddress?.ToString()
                             ?? "anonymous";

                return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 200,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Too many requests. Please try again later.",
                    retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                        ? retryAfter.TotalSeconds
                        : (double?)null
                }, cancellationToken);
            };
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return services;
    }
}
