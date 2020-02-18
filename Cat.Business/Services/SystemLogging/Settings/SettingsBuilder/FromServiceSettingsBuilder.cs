using System;
using Cat.Common.Converters;

namespace Cat.Business.Services.SystemLogging.Settings.SettingsBuilder
{
    public class FromServiceSettingsBuilder : SettingsBuilder
    {

        private ISystemLoggingServiceBase _loggingService;
        private TimeSpan? _cleanThreshold;

        public FromServiceSettingsBuilder(ISystemLoggingServiceBase loggingService, TimeSpan? cleanThreshold)
        {
            SetData(loggingService, cleanThreshold);
        }

        public void SetData(ISystemLoggingServiceBase loggingService, TimeSpan? cleanThreshold)
        {
            _loggingService = loggingService;
            _cleanThreshold = cleanThreshold;
        }

        protected override void BuildSettings()
        {
            _settings.Name = _loggingService.Name();
            _settings.Descriptor = _loggingService.Descriptor();
            _settings.CleanThreshold = _cleanThreshold ?? _loggingService.DefaultCleanThreshold();
            _settings.CleanThresholdString = CatTimeSpanConverter.Convert(_settings.CleanThreshold.Value);
            _settings.IsDefaultCleanThreshold = _settings.CleanThreshold.Value == _loggingService.DefaultCleanThreshold();
        }
    }
}
