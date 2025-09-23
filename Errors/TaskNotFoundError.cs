using FluentResults;

namespace Tarefando.Api.Errors
{
    public class TaskNotFoundError : Error
    {
        public TaskNotFoundError(int id) : base($"Task with id {id} not found.")
        {
            Metadata.Add("TaskId", id);
            Reasons.Add(new NotFoundError());
        }
    }
}
