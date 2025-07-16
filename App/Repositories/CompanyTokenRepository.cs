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

    public async Task<CompanyAPITokens?> DeleteAsync(CompanyAPITokens token)
    {
        _context.CompanyAPITokens.Remove(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<CompanyAPITokens?> GetByTokenAsync(string token)
    {
        return await _context.CompanyAPITokens
            .FirstOrDefaultAsync(c => c.ONCApiToken == token);
    }

    public async Task UpdateAsync(CompanyAPITokens record)
    {
        _context.CompanyAPITokens.Update(record);
        await _context.SaveChangesAsync();
    }




}