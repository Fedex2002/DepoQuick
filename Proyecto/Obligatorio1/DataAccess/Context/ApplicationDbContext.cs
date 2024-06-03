using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccess.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<StorageUnit> StorageUnits { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasKey(p => p.Email); 
        modelBuilder.Entity<Promotion>()
            .HasKey(p => p.Id); 
        modelBuilder.Entity<DateRange>()
            .HasKey(d => d.Id); 
        modelBuilder.Entity<Booking>()
            .HasKey(b => b.Id); 
    }
}