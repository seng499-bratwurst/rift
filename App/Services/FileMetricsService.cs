using Rift.Models;
using Rift.Repositories;


namespace Rift.Services;

public class FileMetricsService : IFileMetricsService
{
    private readonly IFileRepository _fileRepository;
    private readonly IMessageFilesRepository _messageFilesRepository;
    private readonly IMessageRepository _messageRepository;

    public FileMetricsService(IFileRepository fileRepository, IMessageFilesRepository messageFilesRepository, IMessageRepository messageRepository)
    {
        _fileRepository = fileRepository;
        _messageFilesRepository = messageFilesRepository;
        _messageRepository = messageRepository;
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

    public async Task<FileMetricTopic> GetFileMetricsByTopicAsync(string topic)
    {
        var files = await _fileRepository.GetAllAsync();
        var result =new FileMetricTopic
        {
            Topic = topic,
            UpVotes = 0,
            DownVotes = 0,
            Usages = 0,
        };

        var messageIds = await _messageRepository.GetMessageIdsContainingTextAsync(topic);

        if (messageIds.Count == 0)
        {
            return result;
        }

        foreach (var file in files)
        {
            var upVotes = await _messageFilesRepository.GetVotes(file.Id, true, messageIds);
            var downVotes = await _messageFilesRepository.GetVotes(file.Id, false, messageIds);
            var usages = await _messageFilesRepository.GetUsages(file.Id, messageIds);
            Console.WriteLine($"File ID: {file.Id}, Name: {file.Name}, UpVotes: {upVotes}, DownVotes: {downVotes}, Usages: {usages}");
            result.UpVotes += upVotes;
            result.DownVotes += downVotes;
            result.Usages += usages;
        }

        Console.WriteLine($"Found {messageIds.Count} messages containing topic '{topic}'");

        foreach (var id in messageIds)
        {
            Console.WriteLine($"Message ID containing topic '{topic}': {id}");
        }

        return result;
    }
}