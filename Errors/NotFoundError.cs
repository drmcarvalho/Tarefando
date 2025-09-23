using FluentResults;

namespace Tarefando.Api.Errors
{
    public class NotFoundError(): Error("The requested resource was not found.")
    {

    }
}
