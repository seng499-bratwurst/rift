using Microsoft.EntityFrameworkCore;
using Rift.Models;

namespace Rift.Repositories;

public class CompanyTokenRepository : ICompanyTokenRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyAPITokens?> GetByCompanyNameAsync(string companyName)
    {
        return await _context.CompanyAPITokens
            .FirstOrDefaultAsync(t => t.CompanyName == companyName);
    }

    public async Task<CompanyAPITokens> AddAsync(CompanyAPITokens token)
    {
        _context.CompanyAPITokens.Add(token);
        await _context.SaveChangesAsync();
        return token;
    }


}