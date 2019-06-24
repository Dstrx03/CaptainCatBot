using System;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Cat.Business.Services.InternalServices;
using Cat.Business.Services.SystemLogging;
using Cat.Common.AppSettings;
using Cat.Domain;
using Cat.Web.Infrastructure.Platform;
using log4net;
using StructureMap;

namespace Cat.Web.App_Start
{
    public class RefresherConfig
    {
        private static readonly HttpClient _client;

        private static AppDbContext _currDbContext;
        private static IContainer _currContainer;

        private static IRefresherManager _refreshManager;
        private static ISystemLoggingServiceBase _loggingService;

        private static RefresherSettings _settings;
        private static readonly string _callUrl;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static RefresherConfig()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _callUrl = BaseUrlProvider.BaseUrl;
        }

        public static void Register()
        {
            //get curent settings
            if (_currDbContext != null) _currDbContext.Dispose();
            _currDbContext = new AppDbContext();
            if (_currContainer != null) _currContainer.Dispose();
            _currContainer = StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer();
            _currContainer.Configure(x =>
            {
                x.For<AppDbContext>().Use(_currDbContext);
                x.For<DbContext>().Use(_currDbContext);
            });
            _refreshManager = _currContainer.GetInstance<IRefresherManager>();
            _loggingService = SystemLoggingServiceFactory.CreateService("RefresherService", _currContainer);
            _settings = _refreshManager.GetSettings();

            //return if worker is already running
            Action cachedWork = HttpContext.Current.Cache["Refresh"] as Action;
            if (cachedWork is Action)
            {
                var updateSvcMsg = string.Format("Update Refresher settings. Enabled: {0}, interval: every {1} minutes, call url: '{2}'", _settings.IsEnabled, _settings.IntervalMinutes, _callUrl);
                _loggingService.AddEntry(updateSvcMsg);
                _log.Debug(updateSvcMsg);
                return;
            }
            else
            {
                var registerSvcMsg = string.Format("Register Refresher instance. Enabled: {0}, interval: every {1} minutes, call url: '{2}'", _settings.IsEnabled, _settings.IntervalMinutes, _callUrl);
                _loggingService.AddEntry(registerSvcMsg);
                _log.Debug(registerSvcMsg);
            }

            //get the worker
            Action work = () =>
            {
                while (true)
                {
                    var startTime = DateTime.Now;
                    while (true)
                    {
                        Thread.Sleep(200);
                        var deltaTime = DateTime.Now - startTime;
                        if (deltaTime.TotalSeconds >= 60 * _settings.IntervalMinutes && _settings.IsEnabled) break;
                    }

                    _log.DebugFormat("Refresh job started, calling url: '{0}'", _callUrl);

                    try
                    {
                        var result = Task.Run<HttpResponseMessage>(async () => await _client.GetAsync(_callUrl)).Result;
                        var successMsg = string.Format("Refresh job completed successfully (url: '{0}', status: {1}{2})", _callUrl, (int)result.StatusCode, result.StatusCode);
                        _loggingService.AddEntry(successMsg);
                        _log.Debug(successMsg);
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = string.Format("Refresh job error (url: '{0}'): {1}", _callUrl, ex);
                        _loggingService.AddEntry(errorMsg);
                        _log.Error(errorMsg);
                    }
                }
            };
            work.BeginInvoke(null, null);

            //add this job to the cache
            HttpContext.Current.Cache.Add(
                "Refresh",
                work,
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                (s, o, r) => { Register(); }
            );
        }

    }
}