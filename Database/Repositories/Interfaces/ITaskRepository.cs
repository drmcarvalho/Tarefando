using Tarefando.Api.Database.Entities;

namespace Tarefando.Api.Database.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        IEnumerable<MyTask> Criteria(string? q = null, bool? isCanceled = null, bool? isCompleted = null);
        void Create(MyTask task);
        MyTask? GetById(int id);
        void Update(MyTask task);        
    }
}
