using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using Tarefando.Api.Database.Dtos.Payload;
using Tarefando.Api.Database.Repositories.Interfaces;
using Tarefando.Api.Errors;

namespace Tarefando.Api.Business.TaskManager
{
    public sealed class UpdateTask(ITaskRepository taskRepository, ILogger<UpdateTask> logger, IMemoryCache memoryCache)
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly ILogger<UpdateTask> _logger = logger;
        private readonly IMemoryCache _memoryCache = memoryCache;

        public Result MarkAsCompleted(int taskId)
        {
            _logger.LogInformation("Marking task {TaskId} as completed", taskId);
            var task = _taskRepository.GetById(taskId);
            if (task is null)
            {
                _logger.LogWarning("Task {TaskId} not found", taskId);
                return Result.Fail(new TaskNotFoundError(taskId));
            }
            task.IsCompleted = true;
            task.UpdatedAt = DateTime.UtcNow;
            _taskRepository.Update(task);
            _logger.LogInformation("Task {TaskId} marked as completed", taskId);
            InvalidateCache();
            return Result.Ok();
        }

        public Result MarkAsCanceled(int taskId)
        {
            _logger.LogInformation("Marking task {TaskId} as canceled", taskId);
            var task = _taskRepository.GetById(taskId);
            if (task is null)
            {
                _logger.LogWarning("Task {TaskId} not found", taskId);
                return Result.Fail(new TaskNotFoundError(taskId));
            }
            task.IsCaceled = true;
            task.UpdatedAt = DateTime.UtcNow;
            _taskRepository.Update(task);
            _logger.LogInformation("Task {TaskId} marked as canceled", taskId);
            InvalidateCache();
            return Result.Ok();
        }

        public Result Update(int taskId, UpdateTaskDto dto)
        {
            _logger.LogInformation("Updating task {TaskId}", dto);
            var task = _taskRepository.GetById(taskId);
            if (task is null)
            {
                _logger.LogWarning("Task {TaskId} not found", taskId);
                return Result.Fail(new TaskNotFoundError(taskId));
            }
            if (task.IsCaceled)
            {
                _logger.LogWarning("Task {TaskId} is canceled and cannot be updated", taskId);
                return Result.Fail("Canceled tasks cannot be updated");
            }
            if (task.IsCompleted)
            {
                _logger.LogWarning("Task {TaskId} is completed and cannot be updated", taskId);
                return Result.Fail("Completed tasks cannot be updated");
            }
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                if (dto.Title.Length > 200)
                {
                    _logger.LogWarning("Task title is too long");
                    return Result.Fail("Task title cannot be longer than 200 characters");
                }
                task.Title = dto.Title;
            }
            else
            {
                _logger.LogWarning("Task title is empty");
                return Result.Fail("Task title cannot be empty");
            }
            if (dto.Description is not null)
            {
                task.Description = dto.Description;
            }
            if (dto.TaskType is not null)
            {
                task.TaskType = dto.TaskType.Value;
            }
            task.UpdatedAt = DateTime.UtcNow;
            _taskRepository.Update(task);
            _logger.LogInformation("Task {TaskId} updated successfully", taskId);
            InvalidateCache();
            return Result.Ok();
        }

        private void InvalidateCache()
        {
            if (_memoryCache is MemoryCache cache && cache.Keys.Any())
            {
                cache.Clear();
            }
        }
    }
}
