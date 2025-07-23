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
        // Normalize the file extension to .md if it's .txt
        file.Name = NormalizeFileExtension(file.Name);
        return await _fileRepository.AddAsync(file);
    }

    /// <summary>
    /// Normalize file extensions to ensure all files are stored as .md format
    /// </summary>
    private static string NormalizeFileExtension(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return fileName;

        // Check if the file has a .txt extension and convert it to .md
        if (fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
        {
            return fileName.Substring(0, fileName.Length - 4) + ".md";
        }

        // If no extension or already .md, return as is
        return fileName;
    }

    public async Task<IEnumerable<FileEntityDto>> GetAllFilesAsync()
    {
        return await _fileRepository.GetAllAsync();
    }

    public async Task<int?> DeleteFileByIdAsync(int fileId)
    {
        return await _fileRepository.DeleteAsync(fileId);
    }

    public async Task<string> ExtractTextAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string text = string.Empty;

        if (extension == ".txt" || extension == ".md")
        {
            using var sr = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
            text = await sr.ReadToEndAsync();
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
            text = sb.ToString();
        }
        return text.Replace("\0", string.Empty);
    }

    public async Task<IEnumerable<FileEntityDto>> GetFilesByTitlesAsync(IEnumerable<string> relevantDocTitles)
    {
        if (relevantDocTitles == null || !relevantDocTitles.Any())
            return Enumerable.Empty<FileEntityDto>();

        return await _fileRepository.GetFilesByNamesAsync(relevantDocTitles);
    }
}