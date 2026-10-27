namespace TaskManagerApi.Services;

using TaskManagerApi.Models.DTOs;

/// <summary>
/// Service layer interface mediating between presentation controllers and the EF Core data context.
/// </summary>
public interface ITaskService
{
    Task<PagedResult<TaskDto>> GetTasksAsync(TaskFilterParameters parameters);
    Task<TaskDto?> GetByIdAsync(int id);
    Task<TaskDto> CreateAsync(CreateTaskDto dto);
    Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteAsync(int id);
}
