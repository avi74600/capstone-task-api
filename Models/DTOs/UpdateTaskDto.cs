namespace TaskManagerApi.Models.DTOs;

using System.ComponentModel.DataAnnotations;
using TaskManagerApi.Models.Enums;

/// <summary>
/// Input DTO for updating an existing task.
/// </summary>
public class UpdateTaskDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 150 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; } = string.Empty;

    public TaskStatus Status { get; set; }

    public TaskPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }
}
