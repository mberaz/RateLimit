namespace RateLimiting.RateLimiter;

public class RateLimitPlan
{
    public string Name { get; set; }
    public string Key { get; set; }
    public int CallsNumber{ get; set; }
}