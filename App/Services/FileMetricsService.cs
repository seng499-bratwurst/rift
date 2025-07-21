using Rift.Models;
using Rift.Repositories;


namespace Rift.Services;

public class FileMetricsService : IFileMetricsService
{
    private readonly IFileRepository _fileRepository;
    private readonly IMessageFilesRepository _messageFilesRepository;

    public FileMetricsService(IFileRepository fileRepository, IMessageFilesRepository messageFilesRepository)
    {
        _fileRepository = fileRepository;
        _messageFilesRepository = messageFilesRepository;
    }

    public async Task<IEnumerable<FileMetric>> GetFileMetricsAsync()
    {
        var files = await _fileRepository.GetAllAsync();
        var result = new List<FileMetric>();

        foreach (var file in files)
        {
            var upVotes = await _messageFilesRepository.GetVotes(file.Id, true);
            var downVotes = await _messageFilesRepository.GetVotes(file.Id, false);
            var usages = await _messageFilesRepository.GetUsages(file.Id);
            result.Add(new FileMetric
            {
                FileId = file.Id,
                FileName = file.Name,
                UpVotes = upVotes,
                DownVotes = downVotes,
                Usages = usages
            });
        }

        return result;
    }
}