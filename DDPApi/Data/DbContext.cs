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
    public DbSet<MachineFault> MachineFaults { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<QualityControlRecord> QualityControlRecords { get; set; }
    public DbSet<InventoryMovement> InventoryMovements { get; set; }
    public DbSet<SupplierProduct> SupplierProducts { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<WorkforcePlanning> WorkforcePlannings { get; set; }
    public DbSet<Positions> Positions { get; set; }
    public DbSet<Stages> Stages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Person ve Position arasındaki ilişkiyi tanımla
        modelBuilder.Entity<Person>()
            .HasOne(p => p.Position)
            .WithMany()  // Positions tablosunun Person tablosuna 1:N ilişkisi
            .HasForeignKey(p => p.PositionId);  // Person tablosundaki PositionId alanı ile ilişkiyi kur

        // Diğer model konfigürasyonlarınız...
    }
}

}