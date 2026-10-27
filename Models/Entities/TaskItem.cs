namespace TaskManagerApi.Models.Entities;

using System.ComponentModel.DataAnnotations;
using TaskManagerApi.Models.Enums;

/// <summary>
/// Database entity representing a work item stored via Entity Framework Core.
/// </summary>
public class TaskItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
