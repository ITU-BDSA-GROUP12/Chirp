
global using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;
    public class ChirpDBContext : DbContext
    {
        
        public ChirpDBContext(DbContextOptions<ChirpDBContext> options)
            : base(options)
        {
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
            .HasIndex(a => a.AuthorId)
            .IsUnique();
        modelBuilder.Entity<Cheep>()
            .HasIndex(c => c.CheepId)
            .IsUnique();
    }

    public DbSet<Author> Authors { get; set; }
        public DbSet<Cheep> Cheeps { get; set; }
    }


