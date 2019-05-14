using System.Web.Configuration;
using Cat.Web.Infrastructure.Schedule;
using Cat.Web.Infrastructure.Schedule.Hangfire;
using Hangfire;
using Owin;

namespace Cat.Web
{
    public class HangfireConfig
    {
        public static void RegisterHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });
            app.UseHangfireServer();

            ScheduledTasksRegistry.Register();
        }
    }
}