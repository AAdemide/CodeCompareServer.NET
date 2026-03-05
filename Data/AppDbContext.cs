using Microsoft.EntityFrameworkCore;
using CodeCompareServer.Models;

namespace CodeCompareServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<AllQuestion> AllQuestions { get; set; }
    public DbSet<UserQuestion> UserQuestions { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionQuestion> CollectionQuestions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
