using LiteDB;
using Tarefando.Api.Database.Entities;
using Tarefando.Api.Database.Repositories.Interfaces;

namespace Tarefando.Api.Database.Repositories
{
    public class TaskRepository(ILiteDatabase database) : ITaskRepository
    {
        private readonly ILiteDatabase _database = database;
        private const string CollectionName = "tasks";

        public IEnumerable<MyTask> Criteria(string? q = null) => _database.GetCollection<MyTask>(CollectionName).Query().Where(x => x.Title.Contains(q!) || (x.Description != null && x.Description.Contains(q!))).ToEnumerable();

        public void Create(MyTask task)
        {
            var collection = _database.GetCollection<MyTask>(CollectionName);
            collection.EnsureIndex(x => x.Title);
            collection.EnsureIndex(x => x.Description);
            collection.Insert(task);
        }

        public MyTask? GetById(int id) => _database.GetCollection<MyTask>(CollectionName).Query().Where(x => x.Id == id).FirstOrDefault();

        public void Update(MyTask task)
        {
            var collection = _database.GetCollection<MyTask>(CollectionName);
            collection.Update(task.Id, task);            
        }
    }
}
