using Rift.Models;

namespace Rift.Services;

public interface IFileService
{
    Task<FileEntity> UploadFileAsync(FileEntity file);
    Task<IEnumerable<FileEntityDto>> GetAllFilesAsync();
    Task<int?> DeleteFileByIdAsync(int fileId);
    Task<FileEntity?> GetFileByIdAsync(int fileId);
    Task<string> ExtractTextAsync(IFormFile file);

    Task<IEnumerable<FileEntityDto>> GetFilesByTitlesAsync(IEnumerable<string> relevantDocTitles);
}