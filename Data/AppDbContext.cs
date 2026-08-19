using Microsoft.EntityFrameworkCore;
using LifestyleAPI.Models;

namespace LifestyleAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //User - unique phone Number
            modelBuilder.Entity<User>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();
            //Enum
            modelBuilder.Entity<User>()
                .Property(c => c.Role)
                .HasConversion<string>();
        }
    }
}