namespace Chirp.Infrastructure
{
    public class ChirpDBContext : DbContext
    {
        // The database context for the Chirp database.
        public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Cheep> Cheeps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Here we set up some rules for the entries in the database, ensuring that the data created is valid.
            // We also set up some indexes to speed up the database queries.
            modelBuilder.Entity<Cheep>()
                .Property(c => c.Text)
                .HasMaxLength(160);

            modelBuilder.Entity<Author>()
                .HasKey(e => e.AuthorId)
                .IsClustered(false);

            modelBuilder.Entity<Author>()
               .HasIndex(e => e.AuthorId)
               .IsUnique()
               .IsClustered(false);

            modelBuilder.Entity<Author>()
                .HasIndex(e => e.Email)
                .IsUnique()
                .IsClustered(true);

        }
    }
}


