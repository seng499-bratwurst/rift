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

        record.Usage = 30;

        await _companyTokenRepository.UpdateAsync(record);
        return true; // For testing purposes, always allow


        // if (record == null)
        // {
        //     return false; // Token not found
        // }
        
        // var now = DateTime.UtcNow;
        // Console.WriteLine("\n\n\nToken incremented2433\n\n\n");

        // if (now - record.LastRequestTime > RATE_LIMIT_WINDOW)
        // {
        //     record.LastRequestTime = now;
        //     record.Usage = 1; // Reset usage count after the rate limit window
        // }
        // else
        // {
        //     if (record.RequestsThisHour >= LIMIT_PER_HOUR)
        //     {
        //         return false; // Rate limit exceeded
        //     }
        //     Console.WriteLine("\n\n\nToken incremented\n\n\n");
        //     record.RequestsThisHour++;
        // }
        Console.WriteLine($"Token usage incremented. Count: {record.Usage}, LastUsed: {record.Usage}");
        await _companyTokenRepository.UpdateAsync(record);
        return true;

    }
}