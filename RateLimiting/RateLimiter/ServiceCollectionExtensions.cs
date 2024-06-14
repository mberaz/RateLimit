namespace RateLimiting.RateLimiter
{
    public static class ServiceCollectionExtensions
    {
        public static void UseRateLimit(this IApplicationBuilder app)
        {
            app.UseMiddleware<RateLimiterMiddleware>();
        }
    }
}
