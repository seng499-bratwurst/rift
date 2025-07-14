using Rift.Models;
using Rift.Repositories;

namespace Rift.Services;

public class CompanyTokenService : ICompanyTokenService
{
    private readonly ICompanyTokenRepository _companyTokenRepository;
    public CompanyTokenService(ICompanyTokenRepository companyTokenRepository)
    {
        _companyTokenRepository = companyTokenRepository;
    }

    public async Task<CompanyAPITokens?> CreateTokenAsync(string companyName, string? providedToken, int usage)
    {
        var existing = await _companyTokenRepository.GetByCompanyNameAsync(companyName);
        if (existing != null)
        {
            return null; // Already exists
        }

        var token = string.IsNullOrWhiteSpace(providedToken)
            ? Guid.NewGuid().ToString()
            : providedToken;

        var entity = new CompanyAPITokens
        {
            CompanyName = companyName,
            ONCApiToken = token,
            Usage = usage
        };

        return await _companyTokenRepository.AddAsync(entity);
    }


    public async Task<CompanyAPITokens?> DeleteTokenAsync(string token)
    {
        var existing = await _companyTokenRepository.GetByTokenAsync(token);
        if (existing == null)
        {
            return null; // Token not found
        }

        return await _companyTokenRepository.DeleteAsync(existing);
    }
}