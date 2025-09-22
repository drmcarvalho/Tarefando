using Tarefando.Api.Database.Enums;

namespace Tarefando.Api.Database.Dtos.Payload
{
    public record NewTaskDto(string Title, string? Description, ETaskType TaskType = ETaskType.Normal);
}
