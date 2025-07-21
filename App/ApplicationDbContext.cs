using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rift.Models;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageEdge> MessageEdges { get; set; }

    public DbSet<CompanyAPITokens> CompanyAPITokens { get; set; }
    public DbSet<MessageFiles> MessageFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the self referencing Message - Prompt relationship to cascade delete
        modelBuilder.Entity<Message>()
            .HasOne(m => m.PromptMessage)
            .WithMany()
            .HasForeignKey(m => m.PromptMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        // OutgoingEdges: when a Message is deleted, delete all MessageEdges where it is the source
        modelBuilder.Entity<MessageEdge>()
            .HasOne(me => me.SourceMessage)
            .WithMany(m => m.OutgoingEdges)
            .HasForeignKey(me => me.SourceMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        // IncomingEdges: when a Message is deleted, delete all MessageEdges where it is the target
        modelBuilder.Entity<MessageEdge>()
            .HasOne(me => me.TargetMessage)
            .WithMany(m => m.IncomingEdges)
            .HasForeignKey(me => me.TargetMessageId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}