using System.Threading.Tasks;
using Cat.Business.Schedule.Tasks;
using log4net;

namespace Telegram.Schedule.Tasks
{
    public class CheckWebhookTask : IScheduledAsyncTask
    {
        private readonly ITelegramService _telegramService;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CheckWebhookTask(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public async Task ExecuteAsync()
        {
            await _telegramService.CheckWebhookAsync();
        }
    }
}
