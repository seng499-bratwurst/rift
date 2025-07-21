using Rift.Models;

namespace Rift.Services;

public interface IFileMetricsService
{
    Task<IEnumerable<FileMetric>> GetFileMetricsAsync();
}