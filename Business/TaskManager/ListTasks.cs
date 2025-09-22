using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using Tarefando.Api.Database.Dtos.Response;
using Tarefando.Api.Database.Repositories.Interfaces;

namespace Tarefando.Api.Business.TaskManager
{
    public sealed class ListTasks(ILogger<ListTasks> logger, ITaskRepository taskRepository, IMemoryCache memoryCache)
    {
        private readonly ILogger<ListTasks> _logger = logger;
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly IMemoryCache _memoryCache = memoryCache;        

        public Result<IEnumerable<TaskDto>> Criteria(string? q = null)
        {
            var cacheKey = $"{nameof(ListTasks)}:{nameof(Criteria)}:{q}";
            _logger.LogInformation("Listing all tasks");
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<TaskDto>? cachedTasks) && cachedTasks is not null)
            {
                _logger.LogInformation("Returning cached tasks");
                return Result.Ok(cachedTasks);
            }
            var collection = _taskRepository.Criteria(q)
                .Select(task => new TaskDto {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    IsCaceled = task.IsCaceled,
                    CreatedAt = task.CreatedAt,
                    UpdatedAt = task.UpdatedAt,
                    Type = task.TaskType
                }
            );            
            _memoryCache.Set(cacheKey, collection, TimeSpan.FromMinutes(5));            
            return Result.Ok(collection);
        }
    }
}
