using Tarefando.Api.Database.Enums;

namespace Tarefando.Api.Database.Dtos.Response
{
    public class TaskDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCaceled { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ETaskType? TaskType { get; set; }
        public string? TaskTypeString { get => TaskTypeToString(TaskType); }
        public static string? TaskTypeToString(ETaskType? taskType) => taskType?.ToFriendlyString();
    }
}
