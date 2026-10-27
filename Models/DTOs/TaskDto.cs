namespace TaskManagerApi.Models.DTOs;

using TaskManagerApi.Models.Enums;

/// <summary>
/// Output representation of a TaskItem entity.
/// </summary>
public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public TaskPriority Priority { get; set; }
    public string PriorityName => Priority.ToString();
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
