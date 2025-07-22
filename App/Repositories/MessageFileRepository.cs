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

        public async Task<int> GetVotes(int fileId, bool isHelpful, List<int>? messageIds = null)
        {

            var query = _context.MessageFiles
            .Where(mf => mf.FileId == fileId);

            if (messageIds != null && messageIds.Count > 0)
            {
                query = query.Where(mf => messageIds.Contains(mf.MessageId));
            }

            var result = await query
                .Join(_context.Messages,
                      mf => mf.MessageId,
                      m => m.Id,
                      (mf, m) => m)
                .CountAsync(m => m.IsHelpful == isHelpful);

            return result;
        }

        public async Task<int> GetUsages(int fileId, List<int>? messageIds = null)
        {
            var query = _context.MessageFiles.AsQueryable();

            query = query.Where(mf => mf.FileId == fileId);

            if (messageIds != null && messageIds.Count > 0)
            {
                query = query.Where(mf => messageIds.Contains(mf.MessageId));
            }

            return await query.CountAsync();
        }
    }
}