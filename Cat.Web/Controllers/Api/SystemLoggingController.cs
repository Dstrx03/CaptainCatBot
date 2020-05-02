using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Cat.Business.Services.SystemLogging;
using Cat.Business.Services.SystemLogging.Factory;
using Cat.Business.Services.SystemLogging.Settings;
using Cat.Business.Services.SystemLogging.Settings.SettingsModel;
using Cat.Domain.Entities.SystemLog;
using Cat.Web.Infrastructure.Roles;
using Cat.Web.Infrastructure.Roles.Attributes;
using StructureMap;

namespace Cat.Web.Controllers.Api
{
    [AppAuthorize(AppRole.Admin)]
    public class SystemLoggingController : ApiController
    {
        private readonly IContainer _container;
        private readonly ISystemLoggingSettingsManager _loggingSettingsManager;

        public SystemLoggingController(IContainer container, ISystemLoggingSettingsManager loggingSettingsManager)
        {
            _container = container;
            _loggingSettingsManager = loggingSettingsManager;
        }

        [HttpGet]
        public async Task<List<SystemLogEntry>> GetEntries(string descriptor)
        {
            var loggingService = SystemLoggingServiceFactory.CreateService(descriptor, _container);
            return await loggingService.GetEntriesAsync();
        }

        [HttpGet]
        public async Task<SystemLogEntriesPackage> GetNextEntries(string descriptor, string lastEntryId, int count)
        {
            var loggingService = SystemLoggingServiceFactory.CreateService(descriptor, _container);
            return await loggingService.GetNextEntriesAsync(lastEntryId, count);
        }

        [HttpPost]
        public async Task<SystemLogEntriesPackage> Clean(string descriptor, int secondsThreshold, int prevLoadedCount)
        {
            var loggingService = SystemLoggingServiceFactory.CreateService(descriptor, _container);
            await loggingService.CleanAsync(TimeSpan.FromSeconds(secondsThreshold));
            var result = await loggingService.GetNextEntriesAsync(null, prevLoadedCount);
            return result;
        }

        [HttpGet]
        public async Task<List<SystemLoggingSettings>> GetSettings()
        {
            return await _loggingSettingsManager.GetAllSettings();
        }

        [HttpPut]
        public async Task<List<SystemLoggingSettings>> UpdateSettings([ModelBinder(typeof(SystemLoggingSettingsModelBinder))] List<SystemLoggingSettings> settings)
        {
            foreach (var s in settings)
            {
                await _loggingSettingsManager.UpdateSettingsAsync(s);
            }
            return await _loggingSettingsManager.GetAllSettings();
        }

        [HttpPut]
        public async Task<SystemLoggingSettings> ResetCleanThreshold(string descriptor)
        {
            return await _loggingSettingsManager.ResetSettingsAsync(SystemLoggingServiceFactory.CreateService(descriptor, _container));
        }

    }
}
