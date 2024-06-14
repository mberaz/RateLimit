namespace RateLimiting.RateLimiter
{
    public interface IRateLimitCacheStorage
    {
        int GetCount(string key, DateTime dateTime);
        void UpdateCount(string key, DateTime dateTime, int count = 1);

        RateLimitPlan? GetPlan(string planName, string key);
        void  SavePlan (string planName, string key, RateLimitPlan plan);
    }
}
