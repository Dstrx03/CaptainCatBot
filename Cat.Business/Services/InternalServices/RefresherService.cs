using Cat.Domain.Entities.SystemValues;
using System;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Cat.Business.Services.SystemLogging;
using Cat.Common.AppSettings.Providers;
using Cat.Domain;
using log4net;
using StructureMap;

namespace Cat.Business.Services.InternalServices
{
    public interface IRefresherService
    {
        RefresherSettings GetSettings();
        Task<RefresherSettings> GetSettingsAsync();
        RefresherSettings SaveSettings(RefresherSettings settings);
        Task<RefresherSettings> SaveSettingsAsync(RefresherSettings settings);
    }

    public class RefresherService : IRefresherService
    {
        private const string CachedWorkName = "RefresherService.Refresh";

        private const string SettingsIsEnabledName = "Refresher.IsEnabled";
        private const string SettingsIntervalMinutesName = "Refresher.IntervalMinutes";

        private const bool SettingsIsEnabledDefault = false;
        private const int SettingsIntervalMinutesDefault = 15;

        private static readonly HttpClient _client;

        private static AppDbContext _currDbContext;
        private static IContainer _currContainer;

        private static IRefresherService _refresherService;
        private static ISystemLoggingServiceBase _loggingService;

        private static RefresherSettings _settings;
        private static readonly string _callUrl;

        private readonly ISystemValuesManager _valuesManager;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static RefresherService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _callUrl = string.Format("{0}api/v1/Reverberation/Refresher", BaseUrlProvider.HttpBaseUrl);
        }

        public static void RunRefresher(IContainer container)
        {
            InitSettings(container);
            if (CheckCachedWork()) return;
            CacheWork(container, RegisterWork());
        }

        #region RunRefresher() private methods
        private static void InitSettings(IContainer container)
        {
            if (_currDbContext != null) _currDbContext.Dispose();
            _currDbContext = new AppDbContext();
            if (_currContainer != null) _currContainer.Dispose();
            _currContainer = container;
            _currContainer.Configure(x =>
            {
                x.For<AppDbContext>().Use(_currDbContext);
                x.For<DbContext>().Use(_currDbContext);
            });
            _refresherService = _currContainer.GetInstance<IRefresherService>();
            _loggingService = SystemLoggingServiceFactory.CreateService("RefresherService", _currContainer);
            _settings = _refresherService.GetSettings();
        }

        private static bool CheckCachedWork()
        {
            Timer cachedWork = HttpContext.Current.Cache[CachedWorkName] as Timer;
            var isCached = cachedWork is Timer;
            var message = isCached ? 
                string.Format("Update Refresher settings. Enabled: {0}, interval: every {1} minutes, call url: '{2}'", _settings.IsEnabled, _settings.IntervalMinutes, _callUrl) : 
                string.Format("Register Refresher instance. Enabled: {0}, interval: every {1} minutes, call url: '{2}'", _settings.IsEnabled, _settings.IntervalMinutes, _callUrl);

            _loggingService.AddEntry(message);
            _log.Debug(message);

            return isCached;
        }

        private static Timer RegisterWork()
        {
            var startTime = DateTime.Now;
            var workCallback = new TimerCallback(async obj =>
            {
                var deltaTime = DateTime.Now - startTime;
                if (deltaTime.TotalSeconds < 60 * _settings.IntervalMinutes) return;
                startTime = startTime.AddMinutes(_settings.IntervalMinutes);
                if (!_settings.IsEnabled) return;
                await RefreshAsync();
            });
            return new Timer(workCallback, null, 0, 200);
        }

        private static void CacheWork(IContainer container, Timer work)
        {
            HttpContext.Current.Cache.Add(
                CachedWorkName,
                work,
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                (s, o, r) => { RunRefresher(container); }
            );
        }

        private static async Task RefreshAsync()
        {
            _log.DebugFormat("Refresh job started, calling url: '{0}'", _callUrl);
            try
            {
                var result = await _client.GetAsync(_callUrl);
                var resultString = await result.Content.ReadAsStringAsync();
                var successMsg = string.Format("Refresh job completed successfully (url: '{0}', status: {1}{2}, content: {3})", _callUrl, (int)result.StatusCode, result.StatusCode, resultString);
                await _loggingService.AddEntryAsync(successMsg);
                _log.Debug(successMsg);
            }
            catch (Exception ex)
            {
                var errorMsg = string.Format("Refresh job error (url: '{0}'): {1}", _callUrl, ex);
                await _loggingService.AddEntryAsync(errorMsg);
                _log.Error(errorMsg);
            }
        }
        #endregion



        public RefresherService(ISystemValuesManager valuesManager)
        {
            _valuesManager = valuesManager;
        }

        public RefresherSettings GetSettings()
        {
            return CreateRefresherSettings(GetIsEnabled(), GetIntervalMinutes());
        }

        public async Task<RefresherSettings> GetSettingsAsync()
        {
            return CreateRefresherSettings(await GetIsEnabledAsync(), await GetIntervalMinutesAsync());
        }

        public RefresherSettings SaveSettings(RefresherSettings settings)
        {
            _valuesManager.Set(settings.IsEnabled, SettingsIsEnabledName, SystemValueType.Boolean);
            _valuesManager.Set(settings.IntervalMinutes, SettingsIntervalMinutesName, SystemValueType.Int);

            return settings;
        }

        public async Task<RefresherSettings> SaveSettingsAsync(RefresherSettings settings)
        {
            await _valuesManager.SetAsync(settings.IsEnabled, SettingsIsEnabledName, SystemValueType.Boolean);
            await _valuesManager.SetAsync(settings.IntervalMinutes, SettingsIntervalMinutesName, SystemValueType.Int);

            return settings;
        }



        #region Private methods
        private bool GetIsEnabled()
        {
            var isEnabled = _valuesManager.Get(SettingsIsEnabledName);
            if (isEnabled == null)
            {
                isEnabled = SettingsIsEnabledDefault;
                _valuesManager.Set(isEnabled, SettingsIsEnabledName, SystemValueType.Boolean);
            }
            return (bool)isEnabled;
        }

        private async Task<bool> GetIsEnabledAsync()
        {
            var isEnabled = await _valuesManager.GetAsync(SettingsIsEnabledName);
            if (isEnabled == null)
            {
                isEnabled = SettingsIsEnabledDefault;
                await _valuesManager.SetAsync(isEnabled, SettingsIsEnabledName, SystemValueType.Boolean);
            }
            return (bool)isEnabled;
        }

        private int GetIntervalMinutes()
        {
            var intervalMinutes = _valuesManager.Get(SettingsIntervalMinutesName);
            if (intervalMinutes == null)
            {
                intervalMinutes = SettingsIntervalMinutesDefault;
                _valuesManager.Set(intervalMinutes, SettingsIntervalMinutesName, SystemValueType.Int);
            }
            return (int) intervalMinutes;
        }

        private async Task<int> GetIntervalMinutesAsync()
        {
            var intervalMinutes = await _valuesManager.GetAsync(SettingsIntervalMinutesName);
            if (intervalMinutes == null)
            {
                intervalMinutes = SettingsIntervalMinutesDefault;
                await _valuesManager.SetAsync(intervalMinutes, SettingsIntervalMinutesName, SystemValueType.Int);
            }
            return (int)intervalMinutes;
        }

        private RefresherSettings CreateRefresherSettings(bool isEnabled, int intervalMinutes)
        {
            return new RefresherSettings
            {
                IsEnabled = isEnabled,
                IntervalMinutes = intervalMinutes
            };
        }
        #endregion
    }
}
