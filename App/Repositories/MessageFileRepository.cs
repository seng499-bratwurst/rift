using Microsoft.EntityFrameworkCore;
using Rift.Models;

namespace Rift.Repositories
{
    public class MessageFilesRepository : IMessageFilesRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageFilesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMessageFilesAsync(IEnumerable<MessageFiles> messageFilesList)
        {
            await _context.MessageFiles.AddRangeAsync(messageFilesList);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MessageFiles>> GetMessageFilesByMessageIdsAsync(List<int> messageIds)
        {
            return await _context.MessageFiles
                .Where(mf => messageIds.Contains(mf.MessageId))
                .ToListAsync();
        }
    }
}