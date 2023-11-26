using Microsoft.AspNetCore.Builder;
using ParrotIncSquawk.Middleware;

namespace ParrotIncSquawk.Extensions
{
    public static class RateLimitMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimit(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitMiddleware>();
        }
    }
}
