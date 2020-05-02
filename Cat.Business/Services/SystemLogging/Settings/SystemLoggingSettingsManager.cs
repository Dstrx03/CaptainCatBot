using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cat.Business.Services.SystemLogging.Factory;
using Cat.Business.Services.SystemLogging.Settings.SettingsBuilder;
using Cat.Business.Services.SystemLogging.Settings.SettingsModel;
using Cat.Domain.Entities.SystemValues;
using StructureMap;

namespace Cat.Business.Services.SystemLogging.Settings
{
    public interface ISystemLoggingSettingsManager
    {
        Task<List<SystemLoggingSettings>> GetAllSettings();

        SystemLoggingSettings GetSettings(ISystemLoggingServiceBase loggingService);

        void UpdateSettings(SystemLoggingSettings settings);

        SystemLoggingSettings ResetSettings(ISystemLoggingServiceBase loggingService);

        Task<SystemLoggingSettings> GetSettingsAsync(ISystemLoggingServiceBase loggingService);

        Task UpdateSettingsAsync(SystemLoggingSettings settings);

        Task<SystemLoggingSettings> ResetSettingsAsync(ISystemLoggingServiceBase loggingService);
    }

    public class SystemLoggingSettingsManager : ISystemLoggingSettingsManager
    {
        private readonly IContainer _container;
        private readonly ISystemValuesManager _valuesManager;

        public SystemLoggingSettingsManager(IContainer container, ISystemValuesManager valuesManager)
        {
            _container = container;
            _valuesManager = valuesManager;
        }

        public async Task<List<SystemLoggingSettings>> GetAllSettings()
        {
            var services = SystemLoggingServiceFactory.CreateAllServices(_container);
            var settings = new List<SystemLoggingSettings>();
            foreach (var service in services)
            {
                settings.Add(await GetSettingsAsync(service));
            }
            return settings;
        }

        public SystemLoggingSettings GetSettings(ISystemLoggingServiceBase loggingService)
        {
            var cleanThreshold = (TimeSpan?) _valuesManager.Get(CleanThresholdDescriminator(loggingService.Descriptor()));
            return CreateSettings(loggingService, cleanThreshold);
        }

        public void UpdateSettings(SystemLoggingSettings settings)
        {
            var service = SystemLoggingServiceFactory.CreateService(settings.Descriptor, _container);
            if (settings.CleanThreshold.Value != service.DefaultCleanThreshold())
                _valuesManager.Set(settings.CleanThreshold, CleanThresholdDescriminator(settings.Descriptor), SystemValueType.TimeSpan);
            else
                _valuesManager.Remove(CleanThresholdDescriminator(settings.Descriptor));
        }

        public SystemLoggingSettings ResetSettings(ISystemLoggingServiceBase loggingService)
        {
            _valuesManager.Remove(CleanThresholdDescriminator(loggingService.Descriptor()));
            return GetSettings(loggingService);
        }

        public async Task<SystemLoggingSettings> GetSettingsAsync(ISystemLoggingServiceBase loggingService)
        {
            var cleanThreshold = (TimeSpan?) await _valuesManager.GetAsync(CleanThresholdDescriminator(loggingService.Descriptor()));
            return CreateSettings(loggingService, cleanThreshold);
        }

        public async Task UpdateSettingsAsync(SystemLoggingSettings settings)
        {
            var service = SystemLoggingServiceFactory.CreateService(settings.Descriptor, _container);
            if (settings.CleanThreshold.Value != service.DefaultCleanThreshold())
                await _valuesManager.SetAsync(settings.CleanThreshold, CleanThresholdDescriminator(settings.Descriptor), SystemValueType.TimeSpan);
            else 
                await _valuesManager.RemoveAsync(CleanThresholdDescriminator(settings.Descriptor));
        }

        public async Task<SystemLoggingSettings> ResetSettingsAsync(ISystemLoggingServiceBase loggingService)
        {
            await _valuesManager.RemoveAsync(CleanThresholdDescriminator(loggingService.Descriptor()));
            return await GetSettingsAsync(loggingService);
        }

        private SystemLoggingSettings CreateSettings(ISystemLoggingServiceBase loggingService, TimeSpan? cleanThreshold)
        {
            var settingBuilder = new FromServiceSettingsBuilder(loggingService, cleanThreshold);
            return settingBuilder.GetResult();
        }

        private string CleanThresholdDescriminator(string loggingServiceDescriptor)
        {
            return string.Format("{0}LoggingService_Settings.CleanThreshold", loggingServiceDescriptor);
        }
    }
}
