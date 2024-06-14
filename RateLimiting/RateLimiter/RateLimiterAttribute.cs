using Microsoft.AspNetCore.Mvc.Filters;

namespace RateLimiting.RateLimiter
{
    public class RateLimiterAttribute : ActionFilterAttribute
    {
        public string Plan { get; set; }

        public RateLimiterAttribute()
        {

        }

        public RateLimiterAttribute(string plan)
        {
            Plan = plan;
        }
    }
}
