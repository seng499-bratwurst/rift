using Rift.Models;

namespace Rift.Repositories
{
    public interface IMessageFilesRepository
    {
        Task AddMessageFilesAsync(IEnumerable<MessageFiles> messageFilesList);
        Task<List<MessageFiles>> GetMessageFilesByMessageIdsAsync(List<int> messageIds);
        Task<int> GetVotes(int fileId, bool isHelpful);
        Task<int> GetUsages(int fileId);
    }
}