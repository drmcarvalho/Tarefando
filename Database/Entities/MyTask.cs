using Tarefando.Api.Database.Enums;

namespace Tarefando.Api.Database.Entities
{
    public class MyTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; } = null;
        public bool IsCompleted { get; set; } = false;
        public bool IsCaceled { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ETaskType TaskType { get; set; } = ETaskType.Normal;
    }
}
