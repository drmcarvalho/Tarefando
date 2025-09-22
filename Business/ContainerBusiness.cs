using Tarefando.Api.Business.TaskManager;

namespace Tarefando.Api.Business
{
    public static class ContainerBusiness
    {
        public static void AddBusiness(this IServiceCollection services)
        {
            services.AddTransient<ListTasks>();
            services.AddTransient<NewTask>();
            services.AddTransient<UpdateTask>();
        }
    }
}
