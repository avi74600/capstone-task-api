namespace TaskManagerApi.Data;

using TaskManagerApi.Models.Entities;
using TaskManagerApi.Models.Enums;

/// <summary>
/// Seeds realistic demo tasks if the database has not yet been populated.
/// </summary>
public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Tasks.Any())
        {
            return; // DB already has data
        }

        var sampleTasks = new List<TaskItem>
        {
            new TaskItem
            {
                Title = "Design Database Schema",
                Description = "Draft relational ERD and define entity relationships for task management domain.",
                Status = TaskStatus.Completed,
                Priority = TaskPriority.High,
                DueDate = DateTime.UtcNow.AddDays(-2),
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new TaskItem
            {
                Title = "Implement EF Core Migrations",
                Description = "Create initial migration and configure code-first SQLite relational database context.",
                Status = TaskStatus.Completed,
                Priority = TaskPriority.High,
                DueDate = DateTime.UtcNow.AddDays(-1),
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new TaskItem
            {
                Title = "Build Task Service Layer",
                Description = "Encapsulate business logic, data filtering, and pagination into ITaskService interface.",
                Status = TaskStatus.InProgress,
                Priority = TaskPriority.Urgent,
                DueDate = DateTime.UtcNow.AddDays(2),
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new TaskItem
            {
                Title = "Configure Swagger OpenAPI UI",
                Description = "Enable XML doc generation, endpoint descriptions, and Swagger UI at root route.",
                Status = TaskStatus.InProgress,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.UtcNow.AddDays(3),
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new TaskItem
            {
                Title = "Write Unit Tests for Pagination",
                Description = "Test edge cases for PageNumber, PageSize bounds, and total pages calculations.",
                Status = TaskStatus.Pending,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.UtcNow.AddDays(5),
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new TaskItem
            {
                Title = "Setup CI/CD Pipeline",
                Description = "Configure GitHub Actions workflow for automated dotnet build and test runs on push.",
                Status = TaskStatus.Pending,
                Priority = TaskPriority.Low,
                DueDate = DateTime.UtcNow.AddDays(10),
                CreatedAt = DateTime.UtcNow
            },
            new TaskItem
            {
                Title = "Performance Profiling",
                Description = "Benchmark SQL query execution plans for indexed task status and priority filters.",
                Status = TaskStatus.Pending,
                Priority = TaskPriority.Low,
                DueDate = DateTime.UtcNow.AddDays(14),
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Tasks.AddRangeAsync(sampleTasks);
        await context.SaveChangesAsync();
    }
}
