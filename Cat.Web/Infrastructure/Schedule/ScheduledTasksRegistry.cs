using Cat.Business.Schedule.Tasks;
using Cat.Web.Infrastructure.Schedule.Tasks;
using Hangfire;

namespace Cat.Web.Infrastructure.Schedule
{
    public class ScheduledTasksRegistry
    {
        public static void Register()
        {
            AddRecurring<TestTask>(Cron.Hourly());
        }

        private static void AddRecurring<T>(string cronExpression) where T : IScheduledTask
        {
            RecurringJob.AddOrUpdate<ScheduledTask<T>>(x => x.Execute(), cronExpression);
        }
    }
}