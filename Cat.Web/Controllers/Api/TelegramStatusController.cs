using System.Threading.Tasks;
using System.Web.Http;
using Cat.Business.Services.SystemLogging;
using Cat.Business.Services.SystemLogging.Factory;
using Cat.Common.AppSettings;
using Cat.Common.AppSettings.Providers;
using Cat.Web.Infrastructure.Roles;
using Cat.Web.Infrastructure.Roles.Attributes;
using log4net;
using StructureMap;
using Telegram;
using Telegram.Bot.Types;
using Telegram.Models;

namespace Cat.Web.Controllers.Api
{
    [AppAuthorize(AppRole.Admin)]
    public class TelegramStatusController : ApiController
    {
        private readonly ITelegramService _telegramService;
        private readonly IContainer _container;
        private readonly ISystemLoggingServiceBase _loggingService;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TelegramStatusController(ITelegramService telegramService, IContainer container)
        {
            _telegramService = telegramService;
            _container = container;
            _loggingService = SystemLoggingServiceFactory.CreateService(ServiceType.TelegramBot, container);
        }

        [HttpGet]
        public TelegramStatusViewModel GetTelegramStatusInfo()
        {
            return _telegramService.GetTelegramStatusInfo();
        }

        [HttpPost]
        public async Task RegisterClient()
        {
            await TelegramBot.RegisterClientAsync(TelegramBotTokenProvider.Token, _loggingService);
        }

        [HttpPost]
        public void UnregisterClient()
        {
            TelegramBot.UnregisterClient(_loggingService);
        }

        [HttpPost]
        public async Task RegisterWebhook()
        {
            await TelegramBot.RegisterWebhookAsync(AppSettings.InstanceTelegram.WebhookUrl, AppSettings.InstanceTelegram.NeedPublicCert, _loggingService);
        }

        [HttpPost]
        public async Task UnregisterWebhook()
        {
            await TelegramBot.UnregisterWebhookAsync(_loggingService);
        }

        [HttpPost]
        public async Task CheckWebhook()
        {
            await _telegramService.CheckWebhookAsync();
        }

        [HttpPost]
        public async Task<WebhookInfo> UpdateWebhook()
        {
            return await TelegramBot.UpdateWebhookInfoAsync(_loggingService);
        }

    }
}
