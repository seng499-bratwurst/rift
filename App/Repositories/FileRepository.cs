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
                Name = file.Name,
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

        public async Task<List<FileEntityDto>> GetDocumentsByIdsAsync(List<int> ids)
        {
            return await _dbContext.Files
                .Where(f => ids.Contains(f.Id))
                .Select(f => new FileEntityDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    CreatedAt = f.CreatedAt,
                    UploadedBy = f.UploadedBy,
                    SourceLink = f.SourceLink,
                    SourceType = f.SourceType
                })
                .ToListAsync();
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

        public async Task<IEnumerable<FileEntityDto>> GetFilesByNamesAsync(IEnumerable<string> names)
        {
            var nameSet = new HashSet<string>(names);
            var result = await _dbContext.Files
                .Where(f => nameSet.Contains(f.Name))
                .Select(f => new FileEntityDto
                {
                    Name = f.Name,
                    Id = f.Id,
                    CreatedAt = f.CreatedAt,
                    UploadedBy = f.UploadedBy,
                    SourceLink = f.SourceLink,
                    SourceType = f.SourceType
                })
                .ToListAsync();
            return result;
        }
    }
}