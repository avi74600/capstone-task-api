namespace TaskManagerApi.Models.DTOs;

using System.ComponentModel.DataAnnotations;
using TaskManagerApi.Models.Enums;

/// <summary>
/// Query parameters for filtering, searching, sorting, and paginating tasks.
/// </summary>
public class TaskFilterParameters
{
    /// <summary>
    /// Filter by status (Pending, InProgress, Completed, Cancelled).
    /// </summary>
    public TaskStatus? Status { get; set; }

    /// <summary>
    /// Filter by priority (Low, Medium, High, Urgent).
    /// </summary>
    public TaskPriority? Priority { get; set; }

    /// <summary>
    /// Case-insensitive search string evaluated against task title and description.
    /// </summary>
    [MaxLength(100)]
    public string? Search { get; set; }

    /// <summary>
    /// Property name to sort by (e.g., "title", "dueDate", "priority", "createdAt"). Defaults to "createdAt".
    /// </summary>
    public string? SortBy { get; set; } = "createdAt";

    /// <summary>
    /// Sort direction. Defaults to true (descending).
    /// </summary>
    public bool SortDescending { get; set; } = true;

    /// <summary>
    /// 1-based page index.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than or equal to 1.")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page (maximum 50).
    /// </summary>
    [Range(1, 50, ErrorMessage = "PageSize must be between 1 and 50.")]
    public int PageSize { get; set; } = 10;
}
