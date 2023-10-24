namespace Chirp.Infrastructure
{
    public class ChirpDBContext : DbContext
    {
        public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Cheep> Cheeps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cheep>()
                .Property(c => c.Text)
                .HasMaxLength(280);
        }
    }
}


