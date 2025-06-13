using Microsoft.EntityFrameworkCore;
using Rift.Models;

public class FileDbContext: DbContext
{
    public FileDbContext(DbContextOptions<FileDbContext> options) : base(options) { }
    public DbSet<FileEntity> Files { get; set; }
}