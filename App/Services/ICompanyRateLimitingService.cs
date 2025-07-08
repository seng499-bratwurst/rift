public interface ICompanyRateLimitingService
{
    Task<bool> IsAllowedAsync(string token);
}
