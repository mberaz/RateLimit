using Microsoft.AspNetCore.Http.Features;
using System.Net;
using System.Numerics;

namespace RateLimiting.RateLimiter;

public class RateLimiterMiddleware(RequestDelegate next,
    IRateLimitStorage storage,
    IKeyExtractor keyExtractor)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;

        var rateLimiterAttribute = endpoint?.Metadata.GetMetadata<RateLimiterAttribute>();
        var tooManyRequests = false;
        if (rateLimiterAttribute != null)
        {
            var key = keyExtractor.ExtractKey(context);
            var plan = await storage.GetPlan(rateLimiterAttribute.Plan, key);
            var currentCount = await storage.GetCurrentCount(plan.Name, key);

            if (currentCount >= plan.CallsNumber)
            {
                tooManyRequests = true;
            }
            else
            {
                await storage.UpdateCount(plan.Name, key, currentCount + 1);
            }
        }

        if (tooManyRequests)
        {
            await HandleExceptionAsync(context);
        }
        else
        {
            await next(context);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;

        return httpContext.Response.WriteAsync("Too Many Requests");
    }
}