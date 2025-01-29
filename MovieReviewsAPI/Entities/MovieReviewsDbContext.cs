using Microsoft.EntityFrameworkCore;
using MovieReviewsAPI.Entities;
namespace MovieReviewsAPI.Entities
{
    public class MovieReviewsDbContext : DbContext
    {
        public MovieReviewsDbContext(DbContextOptions<MovieReviewsDbContext> options) : base(options) { }
        
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Director> Director { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Review> Review { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasData(new Role() { Id = 1, Name = "User" },
                new Role() { Id = 2, Name = "Rewiever" },
                new Role() { Id = 3, Name = "Moderator"},
                new Role() { Id = 4, Name = "Admin"});

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
