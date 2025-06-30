using Rift.Models;

namespace Rift.Services;

public interface ICompanyTokenService
{
    Task<CompanyAPITokens?> CreateTokenAsync(string companyName, string? providedToken, int usage);
}
