namespace RateLimiting.RateLimiter;

public class RateLimitStorage : IRateLimitStorage
{
    private readonly IRateLimitCacheStorage _storage;

    public RateLimitStorage(IRateLimitCacheStorage storage)
    {
        _storage = storage;
    }

    public Task<RateLimitPlan> GetPlan(string planName, string key)
    {
        //get from the DB
        var cachedPlan = _storage.GetPlan(planName, key);
        if (cachedPlan != null)
        {
            return Task.FromResult(cachedPlan);
        }

        var allPlans = PlanList();
        var plans = allPlans.Where(p => p.Name == planName &&
                                        (p.Key == key || p.Key == "*")).ToList();
        var plan = plans.Count == 1
            ? plans.First()//no specific plan for this key, just the default plan 
            : plans.First(p => p.Key == key);

        _storage.SavePlan(planName, key, plan);
        return Task.FromResult(plan);
    }

    public Task<int> GetCurrentCount(string planName, string key)
    {
        var startOfMinute = GetStartOfMinute();
        var count = _storage.GetCount(planName + "_" + key, startOfMinute);
        return Task.FromResult(count);
    }

    public Task UpdateCount(string planName, string key, int count)
    {
        var startOfMinute = GetStartOfMinute();
        _storage.UpdateCount(planName + "_" + key, startOfMinute, count);
        return Task.CompletedTask;
    }

    private static DateTime GetStartOfMinute()
    {
        var startOfMinute =
            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                DateTime.Now.Hour, DateTime.Now.Minute, 0);
        return startOfMinute;
    }

    private List<RateLimitPlan> PlanList() =>
    [
        new RateLimitPlan
        {
            Name = "a",
            Key = "*",
            CallsNumber = 3
        },

        new RateLimitPlan
        {
            Name = "a",
            Key = "111",
            CallsNumber = 10
        },

        new RateLimitPlan
        {
            Name = "b",
            Key = "*",
            CallsNumber = 5
        }
    ];
}