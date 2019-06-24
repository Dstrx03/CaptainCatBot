using System.Web.Http;
using Cat.Business.Services.SystemLogging;
using Cat.Common.AppSettings;
using Cat.Web.Infrastructure.Platform;
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
            _loggingService = SystemLoggingServiceFactory.CreateService("TelegramBot", container);
        }

        [HttpGet]
        public TelegramStatusViewModel GetTelegramStatusInfo()
        {
            return _telegramService.GetTelegramStatusInfo();
        }

        [HttpPost]
        public void RegisterClient()
        {
            TelegramBot.RegisterClient(TelegramBotTokenProvider.Token, _loggingService);
        }

        [HttpPost]
        public void UnregisterClient()
        {
            TelegramBot.UnregisterClient(_loggingService);
        }

        [HttpPost]
        public void RegisterWebhook()
        {
            TelegramBot.RegisterWebhook(AppSettings.InstanceTelegram.WebhookUrl, AppSettings.InstanceTelegram.NeedPublicCert, _loggingService);
        }

        [HttpPost]
        public void UnregisterWebhook()
        {
            TelegramBot.UnregisterWebhook(_loggingService);
        }

        [HttpPost]
        public void CheckWebhook()
        {
            _telegramService.CheckWebhook();
        }

        [HttpPost]
        public WebhookInfo UpdateWebhook()
        {
            return TelegramBot.UpdateWebhookInfo(_loggingService);
        }

    }
}
