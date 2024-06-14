namespace RateLimiting.RateLimiter;

public interface IRateLimitStorage
{
    Task<RateLimitPlan> GetPlan(string planName, string key);

    Task<int> GetCurrentCount(string planName,string key);

    Task UpdateCount(string planName,string key, int count);
}