using DDPApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Station> Stations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.StoreId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Unit).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Weight).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedDate).HasColumnType("timestamp");
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamp");
                entity.Property(e => e.PurchaseDate).HasColumnType("timestamp");
                entity.Property(e => e.ExpiryDate).HasColumnType("timestamp");
                entity.Property(e => e.LastInventoryDate).HasColumnType("timestamp");
            });
        }
    }
}
