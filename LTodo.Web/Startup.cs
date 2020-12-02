using LTodo.Web.Common;
using LTodo.Web.IRepository;
using LTodo.Web.Model;
using LTodo.Web.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LTodo.Web
{
    public class Startup
    {
        public IHostEnvironment Environment { get; }

        public Startup(IHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettings(Environment.ContentRootPath));
            services.AddSingleton<LDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddAuthorizationService();
            services.AddControllers();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/MessageHub");
                endpoints.MapControllers();
            });
        }
    }
}
