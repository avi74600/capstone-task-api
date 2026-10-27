namespace TaskManagerApi.Services;

using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models.DTOs;
using TaskManagerApi.Models.Entities;

/// <summary>
/// Service layer implementation handling task business logic, EF Core queries,
/// filtering, sorting, and pagination.
/// </summary>
public class TaskService : ITaskService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TaskService> _logger;

    public TaskService(AppDbContext context, ILogger<TaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<TaskDto>> GetTasksAsync(TaskFilterParameters parameters)
    {
        var query = _context.Tasks.AsNoTracking().AsQueryable();

        // 1. Filtering by Status
        if (parameters.Status.HasValue)
        {
            query = query.Where(t => t.Status == parameters.Status.Value);
        }

        // 2. Filtering by Priority
        if (parameters.Priority.HasValue)
        {
            query = query.Where(t => t.Priority == parameters.Priority.Value);
        }

        // 3. Search across Title and Description
        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            string term = parameters.Search.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(term) || t.Description.ToLower().Contains(term));
        }

        // 4. Dynamic Sorting
        query = parameters.SortBy?.ToLower() switch
        {
            "title" => parameters.SortDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "duedate" => parameters.SortDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
            "priority" => parameters.SortDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
            "status" => parameters.SortDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            _ => parameters.SortDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
        };

        // 5. Total Count for pagination calculation
        int totalCount = await query.CountAsync();

        // 6. Pagination boundaries
        int pageNumber = Math.Max(1, parameters.PageNumber);
        int pageSize = Math.Clamp(parameters.PageSize, 1, 50);

        var entities = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = entities.Select(MapToDto).ToList();

        return new PagedResult<TaskDto>(dtos, totalCount, pageNumber, pageSize);
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Tasks.FindAsync(id);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        var entity = new TaskItem
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim() ?? string.Empty,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _context.Tasks.AddAsync(entity);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created TaskItem #{Id}: '{Title}'", entity.Id, entity.Title);
        return MapToDto(entity);
    }

    public async Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var entity = await _context.Tasks.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        entity.Title = dto.Title.Trim();
        entity.Description = dto.Description?.Trim() ?? string.Empty;
        entity.Status = dto.Status;
        entity.Priority = dto.Priority;
        entity.DueDate = dto.DueDate;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated TaskItem #{Id}: '{Title}'", entity.Id, entity.Title);

        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Tasks.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Tasks.Remove(entity);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted TaskItem #{Id}", id);
        return true;
    }

    private static TaskDto MapToDto(TaskItem item)
    {
        return new TaskDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Status = item.Status,
            Priority = item.Priority,
            DueDate = item.DueDate,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}
