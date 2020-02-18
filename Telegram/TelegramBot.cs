
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using Cat.Business.Services.SystemLogging;
using log4net;
using Microsoft.AspNet.SignalR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Telegram
{
    public class TelegramBot
    {
        private static TelegramBotClient _client;
        private static WebhookInfo _currrentWebhookInfo;
        private static DateTime? _currentWebhookInfoUpdateDate;
        private static bool _clientRegistrationError = false;

        private static readonly IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<TelegramBotHub>();

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static TelegramBotClient Client { get { return _client; } }

        public static WebhookInfo CurrentWebhookInfo { get { return _currrentWebhookInfo; } }

        public static DateTime? CurrentWebhookInfoUpdateDate { get { return _currentWebhookInfoUpdateDate; } }

        public static bool ClientRegistrationError { get { return _clientRegistrationError; } }


        public static void RegisterClient(string token, ISystemLoggingServiceBase loggingService)
        {
            try
            {
                _client = new TelegramBotClient(token);
                _clientRegistrationError = false;

                var tokenValid = Task.Run(async () => await _client.TestApiAsync()).Result;
                if (!tokenValid) throw new Exception(string.Format("TelegramBotClient token '{0}' is invalid", token));

                var successMsg = string.Format("TelegramBotClient registered, token: '{0}'", token);
                loggingService.AddEntry(successMsg);
                _log.Debug(successMsg);
            }
            catch (Exception ex)
            {
                _client = null;
                _currrentWebhookInfo = null;
                _clientRegistrationError = true;

                var errorMsg = string.Format("Error while registering TelegramBotClient: {0}", ex);
                loggingService.AddEntry(errorMsg);
                _log.Error(errorMsg);
            }

            _hubContext.Clients.All.statusInfoUpdated();
        }

        public static void UnregisterClient(ISystemLoggingServiceBase loggingService)
        {
            _client = null;
            _currrentWebhookInfo = null;
            _clientRegistrationError = false;

            var successMsg = "TelegramBotClient unregistered";
            loggingService.AddEntry(successMsg);
            _log.Debug(successMsg);

            _hubContext.Clients.All.statusInfoUpdated();
        }

        public static void RegisterWebhook(string webhookUrl, bool needPublicCert, ISystemLoggingServiceBase loggingService)
        {
            if (_client == null)
            {
                var cannotRegisterMsg = "Cannot register webhook due TelegramBotClient is null";
                loggingService.AddEntry(cannotRegisterMsg);
                _log.Debug(cannotRegisterMsg);
                return;
            }

            try
            {
                if (needPublicCert)
                {
                    var publicCert = new InputFileStream(new FileStream(HostingEnvironment.MapPath("~/App_Data/PublicCert.pem"), FileMode.Open), "PublicCert.pem");
                    _client.SetWebhookAsync(webhookUrl, publicCert).Wait();
                    publicCert.Content.Dispose();
                }
                else
                {
                    _client.SetWebhookAsync(webhookUrl).Wait();
                }

                var successMsg = string.Format("Webhook registered for url '{0}'", webhookUrl);
                loggingService.AddEntry(successMsg);
                _log.Debug(successMsg);
            }
            catch (FileNotFoundException ex)
            {
                var fnfErrMsg = string.Format("Cannot find public certificate: {0}", webhookUrl, ex);
                loggingService.AddEntry(fnfErrMsg);
                _log.Error(fnfErrMsg);
            }
            catch (Exception ex)
            {
                var errorMsg = string.Format("Error while registering webhook for url '{0}': {1}", webhookUrl, ex);
                loggingService.AddEntry(errorMsg);
                _log.Error(errorMsg);
            }

            UpdateWebhookInfo(loggingService);
        }

        public static void UnregisterWebhook(ISystemLoggingServiceBase loggingService)
        {
            if (_client == null)
            {
                var cannotUnregisterMsg = "Cannot unregister webhook due TelegramBotClient is null";
                loggingService.AddEntry(cannotUnregisterMsg);
                _log.Debug(cannotUnregisterMsg);
                return;
            }

            try
            {
                _client.DeleteWebhookAsync().Wait();

                var successMsg = "Webhook unregistered";
                loggingService.AddEntry(successMsg);
                _log.Debug(successMsg);
            }
            catch (Exception ex)
            {
                var errorMsg = string.Format("Error while unregistering webhook: {0}", ex);
                loggingService.AddEntry(errorMsg);
                _log.Error(errorMsg);
            }

            UpdateWebhookInfo(loggingService);
        }

        public static WebhookInfo UpdateWebhookInfo(ISystemLoggingServiceBase loggingService)
        {
            if (_client == null)
            {
                _currrentWebhookInfo = null;

                var cannotRegisterMsg = "Cannot update webhook info due TelegramBotClient is null";
                loggingService.AddEntry(cannotRegisterMsg);
                _log.Debug(cannotRegisterMsg);

                _hubContext.Clients.All.statusInfoUpdated();

                return _currrentWebhookInfo;
            }

            try
            {
                _currrentWebhookInfo = Task.Run(async () => await _client.GetWebhookInfoAsync()).Result;
                _currentWebhookInfoUpdateDate = DateTime.Now;

                var whInfoMsg = _currrentWebhookInfo == null ? 
                    "WebhookInfo update: WebhookInfo is null" : 
                    string.Format("WebhookInfo update: url: '{0}', has custom certificate: {1}, pending update count: {2}, last error date: {3}, last error message: '{4}', max connections: {5}, allowed updates: {6}",
                        _currrentWebhookInfo.Url, _currrentWebhookInfo.HasCustomCertificate, _currrentWebhookInfo.PendingUpdateCount, _currrentWebhookInfo.LastErrorDate, _currrentWebhookInfo.LastErrorMessage, 
                        _currrentWebhookInfo.MaxConnections, _currrentWebhookInfo.AllowedUpdates == null ? "null" : string.Join(", ", _currrentWebhookInfo.AllowedUpdates));
                loggingService.AddEntry(whInfoMsg);
                _log.Debug(whInfoMsg);
            }
            catch (Exception ex)
            {
                _currrentWebhookInfo = null;

                var errorMsg = string.Format("Error while updating WebhookInfo: {0}", ex);
                loggingService.AddEntry(errorMsg);
                _log.Error(errorMsg);
            }

            _hubContext.Clients.All.statusInfoUpdated();

            return _currrentWebhookInfo;
        }

    }

    public class TelegramBotHub : Hub
    {
    }
}
