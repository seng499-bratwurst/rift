using Rift.Models;

namespace Rift.Repositories;

public interface IFileRepository
{
    Task<FileEntity> AddAsync(FileEntity file);
    Task<IEnumerable<FileEntityDto>> GetAllAsync();
    Task<FileEntity?> GetByIdAsync(int fileId);
    Task<int?> DeleteAsync(int fileId);
}
