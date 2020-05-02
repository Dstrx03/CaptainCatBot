
using System;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cat.Business.Services.InternalServices.Settings;
using Cat.Business.Services.SystemLogging;
using Cat.Business.Services.SystemLogging.Factory;
using Cat.Common.AppSettings;
using Cat.Domain;
using log4net;
using StructureMap;

namespace Cat.Business.Services.InternalServices
{

    public class ATriggerService
    {
        private static readonly HttpClient _client;

        private static ATriggerSettingsManager _settingsManager;
        private static ISystemLoggingServiceBase _loggingService;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static ATriggerService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task CallATriggerAsync(IContainer container, string baseUrl, string apiKey, string apiSecret)
        {
            //get curent settings
            var currDbContext = new AppDbContext();
            var currContainer = container;
            currContainer.Configure(x =>
            {
                x.For<AppDbContext>().Use(currDbContext);
                x.For<DbContext>().Use(currDbContext);
            });
            _settingsManager = currContainer.GetInstance<ATriggerSettingsManager>();
            _loggingService = SystemLoggingServiceFactory.CreateService(ServiceType.ATrigger, currContainer);
            var settings = await _settingsManager.GetSettingsAsync();

            var message = string.Format("From '{0}' app; started call at {1} UTC", AppSettings.Instance.AppTitleFormatter.AppTitleFullInternalFormat, DateTime.UtcNow);
            var urlCallback = string.Format("{0}api/v1/Reverberation/ATrigger?message={1}", baseUrl, message);

            var initCallMsg = string.Format("Initializing ATrigger request. Enabled: {0}, callback url: '{1}', callback after {2} minute(s)", settings.IsEnabled, urlCallback, settings.TimeSliceMinutes);
            await _loggingService.AddEntryAsync(initCallMsg);
            _log.Debug(initCallMsg);

            var requestUri = string.Format(
                "https://api.atrigger.com/v1/tasks/create?key={0}&secret={1}&timeSlice={2}minute&count=1&tag_application=start_application&url={3}",
                apiKey, apiSecret, settings.TimeSliceMinutes, urlCallback);

            try
            {
                if (!settings.IsEnabled)
                {
                    var disabledMsg = "Request is ignored due ATrigger requests disabled";
                    await _loggingService.AddEntryAsync(disabledMsg);
                    _log.Debug(disabledMsg);
                    return;
                }

                var result = await _client.GetAsync(requestUri);
                var resultStr = await result.Content.ReadAsStringAsync();
                var successMsg = string.Format("ATrigger request result {0}{1}: {2}", (int)result.StatusCode, result.StatusCode, resultStr);
                await _loggingService.AddEntryAsync(successMsg);
                _log.Debug(successMsg);
            }
            catch (Exception ex)
            {
                var errorMsg = string.Format("ATrigger request error: {0}", ex);
                await _loggingService.AddEntryAsync(errorMsg);
                _log.Error(errorMsg);
            }
            finally
            {
                currDbContext.Dispose();
                currContainer.Dispose();
            }
        }
    }
}
