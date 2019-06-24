using Cat.Business.Schedule.Tasks;
using Cat.Web.Infrastructure.Schedule.Tasks;
using Hangfire;
using Telegram.Schedule.Tasks;

namespace Cat.Web.Infrastructure.Schedule
{
    public class ScheduledTasksRegistry
    {
        public static void Register()
        {
            AddRecurring<CleanUpSystemLogTask>("0 */6 * * *");
            AddRecurring<CheckWebhookTask>("*/10 * * * *");
        }

        private static void AddRecurring<T>(string cronExpression) where T : IScheduledTask
        {
            RecurringJob.AddOrUpdate<ScheduledTask<T>>(x => x.Execute(), cronExpression);
        }
    }
}