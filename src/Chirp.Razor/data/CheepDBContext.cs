using Microsoft.EntityFrameworkCore;



    public class CheepDBContext : DbContext
    {
        
        public CheepDBContext(DbContextOptions<CheepDBContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Cheep> Cheeps { get; set; }
    }


