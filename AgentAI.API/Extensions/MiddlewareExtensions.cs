using AgentAI.API.Middleware;

namespace AgentAI.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApiMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseHttpsRedirection();
        
        app.UseRateLimiter();

        app.UseCors("AllowAll");

        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}
