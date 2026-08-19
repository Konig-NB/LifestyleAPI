using Microsoft.EntityFrameworkCore;
using LifestyleAPI.Models;

namespace LifestyleAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Menu> Menus => Set<Menu>();

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

            // Menu -> Category (many-to-one)
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Menus)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}