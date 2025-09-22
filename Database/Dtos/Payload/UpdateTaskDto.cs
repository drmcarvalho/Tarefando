using Tarefando.Api.Database.Enums;

namespace Tarefando.Api.Database.Dtos.Payload
{
    public record UpdateTaskDto(string Title, string? Description, ETaskType? TaskType);
}
