using AgentAI.Application.Common;
using System.Net;
using System.Text.Json;

namespace AgentAI.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception on {Method} {Path} | TraceId: {TraceId} | Message: {Message} | InnerException: {InnerMessage}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier,
                ex.Message,
                ex.InnerException?.Message ?? "none");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errors = BuildErrorChain(exception);

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "An error occurred while processing your request",
            Errors = errors
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }

    private static List<string> BuildErrorChain(Exception ex)
    {
        var errors = new List<string>();
        var current = ex;
        while (current != null)
        {
            errors.Add(current.Message);
            current = current.InnerException;
        }
        return errors;
    }
}
