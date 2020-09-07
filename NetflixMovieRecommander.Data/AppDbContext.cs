using Microsoft.EntityFrameworkCore;
using NetflixMovieRecommander.Models;

namespace NetflixMovieRecommander.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<WatchItem> WatchItems { get; set; }
        public DbSet<Recommended> Recommendations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rating>()
                .HasOne(x => x.Recommended)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.RecommendedForeignKey);
        }
    }
}