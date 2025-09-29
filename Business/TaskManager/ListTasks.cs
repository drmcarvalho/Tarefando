using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using Tarefando.Api.Database.Dtos.Response;
using Tarefando.Api.Database.Enums;
using Tarefando.Api.Database.Repositories.Interfaces;

namespace Tarefando.Api.Business.TaskManager
{
    public sealed class ListTasks(ILogger<ListTasks> logger, ITaskRepository taskRepository, IMemoryCache memoryCache)
    {
        private readonly ILogger<ListTasks> _logger = logger;
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly IMemoryCache _memoryCache = memoryCache;        

        public Result<IEnumerable<TaskDto>> Criteria(string? q = null, bool? isCanceled = null, bool? isCompleted = null, ETaskType? taskType = null, bool noCache = false)
        {
            var cacheKey = $"{nameof(ListTasks)}:{nameof(Criteria)}:{q}:{isCanceled}:{isCompleted}:{taskType}";
            _logger.LogInformation("Listing all tasks");
            if (!noCache && _memoryCache.TryGetValue(cacheKey, out IEnumerable<TaskDto>? cachedTasks) && cachedTasks is not null)
            {
                _logger.LogInformation("Returning cached tasks");
                return Result.Ok(cachedTasks);
            }
            var collection = _taskRepository.Criteria(q, isCanceled, isCompleted, taskType)
                .SkipWhile(predicate: t => t.IsCaceled)
                .OrderByDescending(t => t.CreatedAt)
                .ThenBy(t => t.TaskType)
                .Select(task => new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    IsCaceled = task.IsCaceled,
                    CreatedAt = task.CreatedAt,
                    UpdatedAt = task.UpdatedAt,
                    TaskType = task.TaskType
                }
            );
            _memoryCache.Set(cacheKey, collection, TimeSpan.FromMinutes(5));            
            return Result.Ok(collection);
        }

        public Result<TaskDto> ById(int id)
        {
            _logger.LogInformation("Getting task by id {Id}", id);
            var task = _taskRepository.GetById(id);
            if (task is null)
            {
                _logger.LogWarning("Task with id {Id} not found", id);
                return Result.Fail(new Errors.TaskNotFoundError(id));
            }
            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                IsCaceled = task.IsCaceled,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                TaskType = task.TaskType
            };
            _logger.LogInformation("Task with id {Id} found", id);
            return Result.Ok(taskDto);
        }

        public Result<IEnumerable<TaskGroupedByDayDto>> GroupedByDayCriteria(string? q = null, bool? isCanceled = null, bool? isCompleted = null, ETaskType? taskType = null, bool noCache = false)
        {
            var cacheKey = $"{nameof(ListTasks)}:{nameof(GroupedByDayCriteria)}:{q}:{isCanceled}:{isCompleted}:{taskType}";
            _logger.LogInformation("Listing all tasks grouped by day");
            if (!noCache && _memoryCache.TryGetValue(cacheKey, out IEnumerable<TaskGroupedByDayDto>? cachedTasks) && cachedTasks is not null)
            {
                _logger.LogInformation("Returning cached grouped tasks");
                return Result.Ok(cachedTasks);
            }
            var collection = _taskRepository.Criteria(q, isCanceled, isCompleted, taskType)
                .OrderByDescending(o => o.CreatedAt)
                .GroupBy(g => g.CreatedAt.Date, (day, g) => new TaskGroupedByDayDto
                {
                    Day = day,
                    Tasks = g.Select(x => new TaskDto
                    {
                        Id = x.Id,
                        CreatedAt = x.CreatedAt,
                        Description = x.Description,
                        IsCaceled = x.IsCaceled,
                        IsCompleted = x.IsCompleted,
                        Title = x.Title,
                        TaskType = x.TaskType,
                        UpdatedAt = x.UpdatedAt
                    })
                    .SkipWhile(predicate: t => t.IsCaceled)
                    .OrderBy(t => t.TaskType)
                }
            );
            _memoryCache.Set(cacheKey, collection, TimeSpan.FromMinutes(5));
            return Result.Ok(collection);
        }
    }
}
