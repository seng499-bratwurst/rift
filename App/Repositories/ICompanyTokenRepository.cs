using Rift.Models;

namespace Rift.Repositories;

public interface ICompanyTokenRepository
{
    Task<CompanyAPITokens?> GetByCompanyNameAsync(string companyName);
    Task<CompanyAPITokens?> GetByTokenAsync(string token);
    Task<CompanyAPITokens> AddAsync(CompanyAPITokens token);
    // Task<CompanyAPITokens?> UpdateAsync(CompanyAPITokens token);
    Task UpdateAsync(CompanyAPITokens token);

}
