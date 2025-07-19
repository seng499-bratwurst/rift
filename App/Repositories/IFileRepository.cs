using Rift.Models;

namespace Rift.Repositories;

public interface IFileRepository
{
    Task<FileEntity> AddAsync(FileEntity file);
    Task<IEnumerable<FileEntityDto>> GetAllAsync();
    Task<FileEntity?> GetByIdAsync(int fileId);
    Task<List<FileEntityDto>> GetDocumentsByIdsAsync(List<int> ids);

    Task<int?> DeleteAsync(int fileId);
    Task<IEnumerable<FileEntityDto>> GetFilesByNamesAsync(IEnumerable<string> names);
}