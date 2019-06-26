using System;
using System.Threading.Tasks;
using Cat.Business.Services;
using Cat.Business.Services.SystemLogging;
using Cat.Common.Helpers;
using log4net;
using StructureMap;
using Telegram.AppSettings;
using Telegram.Bot.Types;
using Telegram.Models;

namespace Telegram
{
    public interface ITelegramService
    {
        TelegramServiceStatus GetClientStatus();

        TelegramServiceStatus GetWebhookStatus();

        TelegramStatusViewModel GetTelegramStatusInfo();

        void CheckWebhook();

        Task ProcessUpdate(Update update);
    }

    public class TelegramService : ITelegramService
    {
        private readonly ITelegramAppSettings _telegramAppSettings;
        private readonly ISystemValuesManager _sysyemValues;
        private readonly ISystemLoggingServiceBase _loggingService;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TelegramService(IContainer container, ITelegramAppSettings telegramAppSettings, ISystemValuesManager sysyemValues)
        {
            _telegramAppSettings = telegramAppSettings;
            _sysyemValues = sysyemValues;
            _loggingService = SystemLoggingServiceFactory.CreateService("TelegramBot", container);
        }

        public TelegramServiceStatus GetClientStatus()
        {
            return TelegramBot.Client == null ? TelegramServiceStatus.Stopped : TelegramServiceStatus.Running;
        }

        public TelegramServiceStatus GetWebhookStatus()
        {
            if (TelegramBot.CurrentWebhookInfo == null) 
                return TelegramServiceStatus.Unknown;
            else if (TelegramBot.CurrentWebhookInfo.Url == _telegramAppSettings.WebhookUrl) 
                return TelegramServiceStatus.Ok;
            else 
                return TelegramServiceStatus.Error;
        }

        public TelegramStatusViewModel GetTelegramStatusInfo()
        {
            var whInfo = TelegramBot.CurrentWebhookInfo;
            TelegramStatusViewModel result = null;
            if (whInfo == null)
            {
                result = new TelegramStatusViewModel
                {
                    TelegramBotClientStatus = GetClientStatus(),
                    WebhookStatus = GetWebhookStatus()
                };
            }
            else
            {
                result = new TelegramStatusViewModel
                {
                    TelegramBotClientStatus = GetClientStatus(),
                    WebhookStatus = GetWebhookStatus(),
                    WebhookUpdateDate = TelegramBot.CurrentWebhookInfoUpdateDate,
                    WebhookUrl =
                        string.IsNullOrEmpty(whInfo.Url) ? string.Empty : MaskDataHelper.MaskWebhookUrl(whInfo.Url),
                    WebhookHasCustomCertificate = whInfo.HasCustomCertificate,
                    WebhookPendingUpdateCount = whInfo.PendingUpdateCount,
                    WebhookLastErrorMessage =
                        string.IsNullOrEmpty(whInfo.LastErrorMessage) ? string.Empty : whInfo.LastErrorMessage,
                    WebhookMaxConnections = whInfo.MaxConnections,
                    WebhookAllowedUpdates =
                        (whInfo.AllowedUpdates == null || whInfo.AllowedUpdates.Length == 0)
                            ? whInfo.MaxConnections > 0 ? "All" : string.Empty
                            : string.Join(", ", whInfo.AllowedUpdates)
                };
                if (whInfo.LastErrorDate > DateTime.MinValue) result.WebhookLastErrorDate = whInfo.LastErrorDate;
            }

            return result;
        }

        public void CheckWebhook()
        {
            var checkMsg = "Checking webhook registry...";
            _loggingService.AddEntry(checkMsg);
            _log.Debug(checkMsg);

            TelegramBot.UpdateWebhookInfo(_loggingService);

            if (TelegramBot.CurrentWebhookInfo == null)
            {
                var errorMsg = "Cannot check webhook registry due current webhook info is null";
                _loggingService.AddEntry(errorMsg);
                _log.Error(errorMsg);
            }
            else if (GetWebhookStatus() != TelegramServiceStatus.Running)
            {
                var needToRegisterMsg = string.Format("Actual webhook url: '{0}', the app webhook url: '{1}'. Registering webhook...", TelegramBot.CurrentWebhookInfo.Url, _telegramAppSettings.WebhookUrl);
                _loggingService.AddEntry(needToRegisterMsg);
                _log.Debug(needToRegisterMsg);

                TelegramBot.RegisterWebhook(_telegramAppSettings.WebhookUrl, _telegramAppSettings.NeedPublicCert, _loggingService);
            }
            else
            {
                var okMsg = string.Format("Actual webhook url: '{0}', the app webhook url: '{1}'. Webhook is registered to this app", TelegramBot.CurrentWebhookInfo.Url, _telegramAppSettings.WebhookUrl);
                _loggingService.AddEntry(okMsg);
                _log.Debug(okMsg);
            }
        }

        public async Task ProcessUpdate(Update update)
        {
            // TODO
        }
    }
}
