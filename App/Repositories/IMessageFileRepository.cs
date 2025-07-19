using Rift.Models;

namespace Rift.Repositories
{
    public interface IMessageFilesRepository
    {
        Task AddMessageFilesAsync(IEnumerable<MessageFiles> messageFilesList);
        Task<List<MessageFiles>> GetMessageFilesByMessageIdsAsync(List<int> messageIds);

    }
}