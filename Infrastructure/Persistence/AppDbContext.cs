using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext :DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Movie>()
            .HasQueryFilter(a => a.DeletedAt == null);

        modelBuilder.Entity<User>()
            .HasQueryFilter(a => a.DeletedAt == null)
            .HasIndex(u => u.Email)
            .IsUnique();

    }
}