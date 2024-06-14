using Microsoft.Extensions.Caching.Memory;

namespace RateLimiting.RateLimiter;

public class RateLimitCacheMemoryStorage : IRateLimitCacheStorage
{
    private readonly IMemoryCache _cache;

    public RateLimitCacheMemoryStorage()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    public int GetCount(string key, DateTime dateTime)
    {
        key = $"{key}_{dateTime.Ticks}";
        var hasValue = _cache.TryGetValue(key, out var count);
        return hasValue ? (int)count : 0;
    }

    public void UpdateCount(string key, DateTime dateTime, int count  )
    {
        key = $"{key}_{dateTime.Ticks}";
        _cache.Set(key, count, absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(5));
    }

    public RateLimitPlan GetPlan(string planName, string key)
    {
        var cacheKey = $"plans_{planName}_{key}";
        var hasValue = _cache.TryGetValue(cacheKey, out var plan);
        return hasValue ? (RateLimitPlan)plan : null;
    }

    public void SavePlan(string planName, string key, RateLimitPlan plan)
    {
        var cacheKey = $"plans_{planName}_{key}";
        _cache.Set(cacheKey, plan, absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(5));
    }

}