using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccess.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    
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
            .HasKey(p => p.Label); 
    }
}