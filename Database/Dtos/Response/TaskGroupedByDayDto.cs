namespace Tarefando.Api.Database.Dtos.Response
{
    public class TaskGroupedByDayDto
    {
        public DateTime? Day { get; set; }
        public IEnumerable<TaskDto> Tasks { get; set; } = [];
    }
}
