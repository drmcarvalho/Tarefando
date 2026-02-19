using LiteDB;
using Tarefando.Api.Database.Entities;
using Tarefando.Api.Database.Enums;
using Tarefando.Api.Database.Repositories.Interfaces;

namespace Tarefando.Api.Database.Repositories
{
    public class TaskRepository(ILiteDatabase database) : ITaskRepository
    {
        private readonly ILiteDatabase _database = database;
        private const string CollectionName = "tasks";

        public IEnumerable<MyTask> Criteria(string? q = null, bool? isCanceled = null, bool? isCompleted = null, ETaskType? taskType = null) =>                    
            _database.GetCollection<MyTask>(CollectionName).Query().Where(x =>
                    (q == null || x.Title.Contains(q) || q == null || (x.Description != null && x.Description.Contains(q))) &&
                    (isCanceled == null || x.IsCaceled == isCanceled.Value) &&
                    (isCompleted == null || x.IsCompleted == isCompleted.Value) &&
                    (taskType == null || x.TaskType == taskType.Value)
                ).ToEnumerable();

        public void Create(MyTask task)
        {
            var collection = _database.GetCollection<MyTask>(CollectionName);
            collection.EnsureIndex(x => x.Title);
            collection.EnsureIndex(x => x.Description);
            collection.EnsureIndex(x => x.IsCaceled);
            collection.EnsureIndex(x => x.IsCompleted);
            collection.EnsureIndex(x => x.TaskType);
            collection.EnsureIndex(x => x.CreatedAt);            
            collection.Insert(task);
        }

        public MyTask? GetById(int id) => 
            _database.GetCollection<MyTask>(CollectionName).Query().Where(x => x.Id == id).FirstOrDefault();

        public void Update(MyTask task)
        {
            var collection = _database.GetCollection<MyTask>(CollectionName);
            collection.Update(task.Id, task);            
        }

        public int CountPending()
        {
            var collection = _database.GetCollection<MyTask>(CollectionName);
            return collection.Query().Where(x => !x.IsCaceled && !x.IsCompleted).Count();
        }
    }
}
