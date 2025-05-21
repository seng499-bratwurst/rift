using Microsoft.EntityFrameworkCore;
using Rift.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Define your DbSets (tables) here
    public DbSet<User> Users { get; set; }
}