using Microsoft.AspNetCore.Mvc;
using Tarefando.Api.Business.TaskManager;
using Tarefando.Api.Database.Dtos.Payload;

namespace Tarefando.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet("criteria")]
        public IActionResult ListTasks([FromServices] ListTasks listAllTasks, [FromQuery] string? q = null, [FromQuery] bool grouped = false)
        {
            if (grouped)
            {
                var resultTasksGrouped = listAllTasks.GroupedByDayCriteria(q);
                return Ok(resultTasksGrouped.ValueOrDefault);
            }
            var resultTasks = listAllTasks.Criteria(q);
            return Ok(resultTasks.ValueOrDefault);
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
                return BadRequest(result.Errors);
            }
            return NoContent();
        }
    }
}
