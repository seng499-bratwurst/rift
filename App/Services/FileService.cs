using Rift.Models;
using Rift.Services;
using Rift.Repositories;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<FileEntity> UploadFileAsync(FileEntity file)
    {
        return await _fileRepository.AddAsync(file);
    }

    public async Task<IEnumerable<FileEntityDto>> GetAllFilesAsync()
    {
        return await _fileRepository.GetAllAsync();
    }

    public async Task<int?> DeleteFileByIdAsync(int fileId)
    {
        return await _fileRepository.DeleteAsync(fileId);
    }

    public async Task<byte[]> ReadFileContentAsync(IFormFile file)
    {
        if (file == null)
            return [];
        using var memoryStream = new System.IO.MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}