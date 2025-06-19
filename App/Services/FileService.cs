using Rift.Models;
using Rift.Services;
using Rift.Repositories;
using System.Text;
using UglyToad.PdfPig;

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

    public async Task<string> ExtractTextAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension == ".txt" || extension == ".md")
        {
            using var sr = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
            return await sr.ReadToEndAsync();
        }
        else if (extension == ".pdf")
        {
            using var pdfStream = file.OpenReadStream();
            using var pdf = PdfDocument.Open(pdfStream);
            var sb = new StringBuilder();
            foreach (var page in pdf.GetPages())
            {
                sb.AppendLine(page.Text);
            }
            return sb.ToString();
        }
        return string.Empty;
    }
}