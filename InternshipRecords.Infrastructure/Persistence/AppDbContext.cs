using System.Reflection;
using InternshipRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipRecords.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Intern> Interns { get; set; } = null!;
    public DbSet<Direction> Directions { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
            switch (entry.Entity)
            {
                case Intern intern:
                {
                    if (entry.State == EntityState.Added)
                        intern.CreatedAt = DateTime.UtcNow;

                    intern.UpdatedAt = DateTime.UtcNow;
                    break;
                }
                case Direction direction:
                {
                    if (entry.State == EntityState.Added)
                        direction.CreatedAt = DateTime.UtcNow;

                    direction.UpdatedAt = DateTime.UtcNow;
                    break;
                }
                case Project project:
                {
                    if (entry.State == EntityState.Added)
                        project.CreatedAt = DateTime.UtcNow;

                    project.UpdatedAt = DateTime.UtcNow;
                    break;
                }
            }

        return base.SaveChangesAsync(cancellationToken);
    }
}