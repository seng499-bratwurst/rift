using Rift.Models;

namespace Rift.Repositories;

public interface ICompanyTokenRepository
{
    Task<CompanyAPITokens?> GetByCompanyNameAsync(string companyName);
    Task<CompanyAPITokens> AddAsync(CompanyAPITokens token);
}
