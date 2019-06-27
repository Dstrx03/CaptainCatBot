using System;
using System.Net;
using System.Threading.Tasks;
using Cat.Business.Services;
using Cat.Business.Services.SystemLogging;
using Cat.Common.Helpers;
using Cat.Domain.Entities.SystemValues;
using log4net;
using StructureMap;
using Telegram.AppSettings;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
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
            var message = update.Message;

            if (update.CallbackQuery != null)
            {
                await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, string.Format("recieved callback: '{0}'", update.CallbackQuery.Data),
                            replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[]{
                                new InlineKeyboardButton()
                                {
                                    Text = "button 1",
                                    CallbackData = "Pressed button1"
                                },
                                new InlineKeyboardButton()
                                {
                                    Text = "button two",
                                    CallbackData = "Pressed button2"
                                },
                                new InlineKeyboardButton()
                                {
                                    Text = "Button 3.0",
                                    CallbackData = "Pressed button3"
                                }
                            }));
            }

            if (message.Type == MessageType.Text)
            {
                var msgText = message.Text.ToLowerInvariant().Trim();

                switch (msgText)
                {
                    case "/test1":
                        await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, "Hello...");
                        await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, "... World!");
                        break;
                    case "/test2":
                        var apiUrl = "http://api.open-notify.org/iss-now.json";

                        using (var client = new WebClient())
                        {
                            try
                            {
                                string data = await client.DownloadStringTaskAsync(apiUrl);
                                await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, string.Format("data: '{0}'", data));
                            }
                            catch (Exception ex)
                            {
                                _log.Debug("client error: " + ex);
                            }
                        }

                        break;
                    case "/test3":
                        await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, "press button",
                            replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[]{
                                new InlineKeyboardButton()
                                {
                                    Text = "button 1",
                                    CallbackData = "Pressed button1"
                                },
                                new InlineKeyboardButton()
                                {
                                    Text = "button two",
                                    CallbackData = "Pressed button2"
                                },
                                new InlineKeyboardButton()
                                {
                                    Text = "Button 3.0",
                                    CallbackData = "Pressed button3"
                                }
                            }));
                        break;
                    case "/subscribe":
                        var dSub = "SubscriptionChatId_" + message.Chat.Id;
                        await _sysyemValues.SetAsync(message.Chat.Id, dSub, SystemValueType.Long);
                        await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, "subscribed!");
                        break;
                    case "/unsubscribe":
                        var dUnsub = "SubscriptionChatId_" + message.Chat.Id;
                        await _sysyemValues.RemoveAsync(dUnsub);
                        await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, "unsubscribed!");
                        break;
                    default:
                        await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, string.Format("echo: '{0}'", msgText));
                        break;
                }
            }
            else
            {
                await TelegramBot.Client.SendTextMessageAsync(message.Chat.Id, string.Format("message type: {0}", message.Type));
            }
        }

    }
}
