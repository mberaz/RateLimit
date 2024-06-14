namespace RateLimiting.RateLimiter
{
    public interface IKeyExtractor
    {
        string ExtractKey(HttpContext context);
    }
}
