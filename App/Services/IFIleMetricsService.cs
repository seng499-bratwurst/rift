using Rift.Models;

namespace Rift.Services;

public interface IFileMetricsService
{
    Task<IEnumerable<FileMetric>> GetFileMetricsAsync();
    Task<FileMetricTopic> GetFileMetricsByTopicAsync(string topic); 
}