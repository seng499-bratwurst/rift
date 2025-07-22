using Rift.Models;

namespace Rift.Repositories
{
    public interface IMessageFilesRepository
    {
        Task AddMessageFilesAsync(IEnumerable<MessageFiles> messageFilesList);
        Task<List<MessageFiles>> GetMessageFilesByMessageIdsAsync(List<int> messageIds);
        Task<int> GetVotes(int fileId, bool isHelpful, List<int>? messageIds = null);
        Task<int> GetUsages(int fileId, List<int>? messageIds = null);
    }
}