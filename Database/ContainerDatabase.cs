using LiteDB;
using Tarefando.Api.Database.Repositories;
using Tarefando.Api.Database.Repositories.Interfaces;

namespace Tarefando.Api.Database
{
    public static class ContainerDatabase
    {
        public static void AddDatabase(this IServiceCollection services)
        {
            BsonMapper.Global.EnumAsInteger = true;
            services.AddSingleton<ILiteDatabase>(_ => new LiteDatabase("tarefandodb.db"));
            services.AddScoped<ITaskRepository, TaskRepository>();
        }
    }
}
