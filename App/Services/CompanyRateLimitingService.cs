using Rift.Repositories;

public class CompanyRateLimitingService : ICompanyRateLimitingService
{
    private readonly ICompanyTokenRepository _companyTokenRepository;
    private const int LIMIT_PER_HOUR = 1000; // can adjust this later
    private static readonly TimeSpan RATE_LIMIT_WINDOW = TimeSpan.FromHours(1);


    public CompanyRateLimitingService(ICompanyTokenRepository companyTokenRepository)
    {
        _companyTokenRepository = companyTokenRepository;
    }

    public async Task<bool> IsAllowedAsync(string token)
    {
        var record = await _companyTokenRepository.GetByTokenAsync(token);
        if (record == null)
        {
            return false; // Token not found
        }
        if (record.Usage >= LIMIT_PER_HOUR)
        {
            return false; // Rate limit exceeded
        }
        record.Usage++;
        await _companyTokenRepository.UpdateAsync(record); // Update usage count
        return true;

    }
}