using Microsoft.EntityFrameworkCore;
using src.Chirp.Razor.Models;

namespace src.Chirp.Razor.Data
{
    public class CheepContext : DbContext
    {
        
        public CheepContext(DbContextOptions<CheepContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Cheep> Cheeps { get; set; }
    }
}

