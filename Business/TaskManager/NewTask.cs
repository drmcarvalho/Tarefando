using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using Tarefando.Api.Database.Dtos.Payload;
using Tarefando.Api.Database.Entities;
using Tarefando.Api.Database.Repositories.Interfaces;

namespace Tarefando.Api.Business.TaskManager
{
    public sealed class NewTask(ITaskRepository taskRepository, ILogger<NewTask> logger, IMemoryCache memoryCache)
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly ILogger<NewTask> _logger = logger;
        private readonly IMemoryCache _memoryCache = memoryCache;

        public Result CreateAsync(NewTaskDto dto)
        {
            _logger.LogInformation("Creating a new task with title: {Title}", dto.Title);
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                _logger.LogWarning("Task title is empty");
                return Result.Fail("Task title cannot be empty");
            }
            if (dto.Title.Length > 200)
            {
                _logger.LogWarning("Task title is too long");
                return Result.Fail("Task title cannot be longer than 200 characters");
            }
            _taskRepository.Create(new MyTask { Title = dto.Title, Description = dto.Description, TaskType = dto.TaskType });
            _logger.LogInformation("Task created successfully");
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
