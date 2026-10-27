namespace TaskManagerApi.Data;

using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models.Entities;

/// <summary>
/// Entity Framework Core database context configuring task entities and relational mapping.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(t => t.Description)
                  .HasMaxLength(1000);

            entity.Property(t => t.Status)
                  .HasConversion<string>()
                  .HasMaxLength(30)
                  .IsRequired();

            entity.Property(t => t.Priority)
                  .HasConversion<string>()
                  .HasMaxLength(30)
                  .IsRequired();

            // Indexes for fast filtering and sorting
            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.Priority);
            entity.HasIndex(t => t.CreatedAt);
        });
    }
}
