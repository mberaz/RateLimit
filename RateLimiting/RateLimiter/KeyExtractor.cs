namespace RateLimiting.RateLimiter;

public class KeyExtractor : IKeyExtractor
{
    public string ExtractKey(HttpContext context)
    {
        var header = context.Request.Headers["x-api-key"];
        return header.FirstOrDefault();
    }
}