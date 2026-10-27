namespace TaskManagerApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Models.DTOs;
using TaskManagerApi.Services;

/// <summary>
/// Presentation controller exposing task management endpoints.
/// Strictly delegates business logic, pagination, and data access to <see cref="ITaskService"/>.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Retrieves a paginated list of tasks with optional filtering by status, priority, or search keyword.
    /// </summary>
    /// <param name="parameters">Filtering, sorting, and pagination options.</param>
    /// <response code="200">Returns the requested page of tasks with pagination metadata.</response>
    /// <response code="400">Query parameters failed validation.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<TaskDto>>> GetAll([FromQuery] TaskFilterParameters parameters)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _taskService.GetTasksAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single task by its integer identifier.
    /// </summary>
    /// <param name="id">Task integer ID.</param>
    /// <response code="200">Task found and returned.</response>
    /// <response code="400">ID is non-positive.</response>
    /// <response code="404">Task with given ID was not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid Identifier",
                Detail = "Task ID must be a positive integer greater than zero.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var task = await _taskService.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Task Not Found",
                Detail = $"Task with ID {id} does not exist.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(task);
    }

    /// <summary>
    /// Creates a new task item with input model validation.
    /// </summary>
    /// <param name="dto">Task creation fields.</param>
    /// <response code="201">Task created successfully with Location header.</response>
    /// <response code="400">Model validation failed on title or parameters.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing task item.
    /// </summary>
    /// <param name="id">Task integer ID to update.</param>
    /// <param name="dto">Updated task fields.</param>
    /// <response code="200">Task successfully updated.</response>
    /// <response code="400">ID is non-positive or payload failed validation.</response>
    /// <response code="404">Task with given ID does not exist.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        if (id <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid Identifier",
                Detail = "Task ID must be greater than zero.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updated = await _taskService.UpdateAsync(id, dto);
        if (updated == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Task Not Found",
                Detail = $"Cannot update. Task with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(updated);
    }

    /// <summary>
    /// Deletes a task item by ID.
    /// </summary>
    /// <param name="id">Task integer ID to delete.</param>
    /// <response code="204">Task successfully deleted.</response>
    /// <response code="400">ID is non-positive.</response>
    /// <response code="404">Task with given ID was not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid Identifier",
                Detail = "Task ID must be greater than zero.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        bool deleted = await _taskService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Task Not Found",
                Detail = $"Cannot delete. Task with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return NoContent();
    }
}
