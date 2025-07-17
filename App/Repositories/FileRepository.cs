using Microsoft.EntityFrameworkCore;
using Rift.Models;

namespace Rift.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileDbContext _dbContext;

        public FileRepository(FileDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FileEntity> AddAsync(FileEntity file)
        {
            _dbContext.Files.Add(file);
            await _dbContext.SaveChangesAsync();
            return file;
        }

        public async Task<IEnumerable<FileEntityDto>> GetAllAsync()
        {
            return await _dbContext.Files
            .Select(file => new FileEntityDto
            {
                Id = file.Id,
                FileName = file.Name,
                CreatedAt = file.CreatedAt,
                UploadedBy = file.UploadedBy,
                SourceLink = file.SourceLink,
                SourceType = file.SourceType
            })
            .ToListAsync();
        }

        public async Task<FileEntity?> GetByIdAsync(int fileId)
        {
            return await _dbContext.Files.FindAsync(fileId);
        }

        public async Task<int?> DeleteAsync(int fileId)
        {
            var file = await _dbContext.Files.FindAsync(fileId);
            if (file != null)
            {
                _dbContext.Files.Remove(file);
                await _dbContext.SaveChangesAsync();
            }
            return file?.Id;
        }
    }
}