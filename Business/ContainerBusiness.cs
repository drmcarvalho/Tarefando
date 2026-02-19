using Tarefando.Api.Business.TaskManager;

namespace Tarefando.Api.Business
{
    public static class ContainerBusiness
    {
        public static void AddBusiness(this IServiceCollection services)
        {
            services.AddTransient<TasksOnlyRead>();
            services.AddTransient<NewTask>();
            services.AddTransient<UpdateTask>();
        }
    }
}
