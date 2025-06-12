using Rift.Models;

namespace Rift.Services;

public interface IFileService
{
    Task<FileEntity> UploadFileAsync(FileEntity file);
    Task<IEnumerable<FileEntityDto>> GetAllFilesAsync();
    Task <int?> DeleteFileByIdAsync(int fileId);

    Task<byte[]> ReadFileContentAsync(IFormFile file);
}