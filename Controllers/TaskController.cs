using Microsoft.AspNetCore.Mvc;
using Tarefando.Api.Business.TaskManager;
using Tarefando.Api.Database.Dtos.Payload;
using Tarefando.Api.Database.Enums;
using Tarefando.Api.Errors;

namespace Tarefando.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : BaseController
    {
        [HttpGet("criteria")]
        public IActionResult ListTasks([FromServices] ListTasks listTasks, [FromQuery] string? q = null, [FromQuery] bool grouped = false, [FromQuery] bool? isCanceled = null, [FromQuery] bool? isCompleted = null, [FromQuery] ETaskType? taskType = null, [FromQuery] bool noCache = false)
        {
            if (grouped)
            {
                var resultTasksGrouped = listTasks.GroupedByDayCriteria(q, isCanceled, isCompleted, taskType, noCache);
                return Ok(resultTasksGrouped.ValueOrDefault);
            }
            var resultTasks = listTasks.Criteria(q, isCanceled, isCompleted, taskType, noCache);
            return Ok(resultTasks.ValueOrDefault);
        }

        [HttpGet("{taskId}")]
        public IActionResult GetTaskById([FromServices] ListTasks listTasks, int taskId, [FromQuery] bool noCache = false)
        {
            var result = listTasks.ById(taskId);
            if (result.IsFailed && result.Errors.Any(e => e is TaskNotFoundError))
            {                
                return NotFound(FormatErrors(result.Errors));                                
            }
            return Ok(result.ValueOrDefault);
        }

        [HttpPost]
        public IActionResult CreateTask([FromServices] NewTask newTask, [FromBody] NewTaskDto dto)
        {
            var result = newTask.CreateAsync(dto);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Created();
        }

        [HttpPut("{taskId}")]
        public IActionResult UpdateTask([FromServices] UpdateTask updateTask, int taskId, [FromBody] UpdateTaskDto dto)
        {
            var result = updateTask.Update(taskId, dto);
            if (result.IsFailed)
            {
                if (result.Errors.Any(e => e is TaskNotFoundError))
                {
                    return NotFound(FormatErrors(result.Errors));
                }
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPatch("complete/{taskId}")]
        public IActionResult MarkTaskAsCompleted([FromServices] UpdateTask updateTask, int taskId)
        {
            var result = updateTask.MarkAsCompleted(taskId);
            if (result.IsFailed)
            {
                if (result.Errors.Any(e => e is TaskNotFoundError))
                {
                    return NotFound(FormatErrors(result.Errors));
                }
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPatch("cancel/{taskId}")]
        public IActionResult MarkTaskAsCanceled([FromServices] UpdateTask updateTask, int taskId)
        {
            var result = updateTask.MarkAsCanceled(taskId);
            if (result.IsFailed)
            {
                if (result.Errors.Any(e => e is TaskNotFoundError))
                {
                    return NotFound(FormatErrors(result.Errors));
                }
                return BadRequest(result.Errors);
            }
            return NoContent();
        }        
    }
}
