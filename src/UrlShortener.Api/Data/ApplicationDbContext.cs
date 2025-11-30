using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Url> Urls { get; set; }
    public DbSet<Click> Clicks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Click>()
            .HasOne(c => c.Url)
            .WithMany(u => u.Clicks)
            .HasForeignKey(c => c.ShortCode)
            .HasPrincipalKey(u => u.ShortCode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
