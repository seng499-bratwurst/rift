using Rift.Models;

namespace Rift.Services
{
    public interface IMessageFilesService
    {
        Task InsertMessageFilesAsync(IEnumerable<FileEntityDto> documents, int id);
    }
}