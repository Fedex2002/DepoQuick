using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccess.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    
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
    }
}