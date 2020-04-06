
using System;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cat.Business.Services.SystemLogging;
using Cat.Common.AppSettings;
using Cat.Common.AppSettings.Providers;
using Cat.Domain;
using Cat.Domain.Entities.SystemValues;
using log4net;
using StructureMap;

namespace Cat.Business.Services.InternalServices
{
    public interface IATriggerService
    {
        ATriggerSettings GetSettings();
        ATriggerSettings SaveSettings(ATriggerSettings settings);
    }

    public class ATriggerService : IATriggerService
    {
        private readonly ISystemValuesManager _valuesManager;

        private static readonly HttpClient _client;

        private static AppDbContext _currDbContext;
        private static IContainer _currContainer;

        private static IATriggerService _atriggerService;
        private static ISystemLoggingServiceBase _loggingService;

        private static ATriggerSettings _settings;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static ATriggerService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void CallATrigger(IContainer container, string baseUrl, string apiKey, string apiSecret)
        {
            //get curent settings
            _currDbContext = new AppDbContext();
            _currContainer = container;
            _currContainer.Configure(x =>
            {
                x.For<AppDbContext>().Use(_currDbContext);
                x.For<DbContext>().Use(_currDbContext);
            });
            _atriggerService = _currContainer.GetInstance<IATriggerService>();
            _loggingService = SystemLoggingServiceFactory.CreateService("ATriggerService", _currContainer);
            _settings = _atriggerService.GetSettings();

            var message = string.Format("From '{0}' app; started call at {1} UTC", AppSettings.Instance.AppTitleFormatter.AppTitleFullInternalFormat, DateTime.UtcNow);
            var urlCallback = string.Format("{0}api/v1/Reverberation/ATrigger?message={1}", baseUrl, message);

            var initCallMsg = string.Format("Initializing ATrigger request. Enabled: {0}, callback url: '{1}', callback after {2} minute(s)", _settings.IsEnabled, urlCallback, _settings.TimeSliceMinutes);
            _loggingService.AddEntry(initCallMsg);
            _log.Debug(initCallMsg);

            var requestUri = string.Format(
                "https://api.atrigger.com/v1/tasks/create?key={0}&secret={1}&timeSlice={2}minute&count=1&tag_application=start_application&url={3}",
                apiKey, apiSecret, _settings.TimeSliceMinutes, urlCallback);

            try
            {
                if (!_settings.IsEnabled)
                {
                    var disabledMsg = "Request is ignored due ATrigger requests disabled";
                    _loggingService.AddEntry(disabledMsg);
                    _log.Debug(disabledMsg);
                    return;
                }
                var result = Task.Run(async () => await _client.GetAsync(requestUri)).Result;
                var resultStr = Task.Run(async () => await result.Content.ReadAsStringAsync()).Result;
                var successMsg = string.Format("ATrigger request result {0}{1}: {2}", (int)result.StatusCode, result.StatusCode, resultStr);
                _loggingService.AddEntry(successMsg);
                _log.Debug(successMsg);
            }
            catch (Exception ex)
            {
                var errorMsg = string.Format("ATrigger request error: {0}", ex);
                _loggingService.AddEntry(errorMsg);
                _log.Error(errorMsg);
            }
            finally
            {
                _currDbContext.Dispose();
                _currContainer.Dispose();
            }
        }



        public ATriggerService(ISystemValuesManager valuesManager)
        {
            _valuesManager = valuesManager;
        }

        public ATriggerSettings GetSettings()
        {
            var isEnabled = _valuesManager.Get("ATrigger.IsEnabled");
            if (isEnabled == null)
            {
                isEnabled = false;
                _valuesManager.Set(isEnabled, "ATrigger.IsEnabled", SystemValueType.Boolean);
            }

            var timeSliceMinutes = _valuesManager.Get("ATrigger.TimeSliceMinutes");
            if (timeSliceMinutes == null)
            {
                timeSliceMinutes = 1;
                _valuesManager.Set(timeSliceMinutes, "ATrigger.TimeSliceMinutes", SystemValueType.Int);
            }

            return new ATriggerSettings
            {
                IsEnabled = (bool)isEnabled,
                TimeSliceMinutes = (int)timeSliceMinutes
            };
        }

        public ATriggerSettings SaveSettings(ATriggerSettings settings)
        {
            _valuesManager.Set(settings.IsEnabled, "ATrigger.IsEnabled", SystemValueType.Boolean);
            _valuesManager.Set(settings.TimeSliceMinutes, "ATrigger.TimeSliceMinutes", SystemValueType.Int);

            return settings;
        }

    }
}
