using Cat.Business.Services.SystemLogging.Settings.SettingsModel;
using Cat.Common.Converters;

namespace Cat.Business.Services.SystemLogging.Settings.SettingsBuilder
{
    public class FromSettingsSettingsBuilder : SettingsBuilder
    {
        private SystemLoggingSettings _originalSettings;

        public FromSettingsSettingsBuilder(SystemLoggingSettings originalSettings)
        {
            SetData(originalSettings);
        }

        public void SetData(SystemLoggingSettings originalSettings)
        {
            _originalSettings = originalSettings;
        }

        protected override void BuildSettings()
        {
            _settings.Name = _originalSettings.Name;
            _settings.Descriptor = _originalSettings.Descriptor;
            _settings.CleanThreshold = CatTimeSpanConverter.Convert(_originalSettings.CleanThresholdString);
            _settings.CleanThresholdString = _originalSettings.CleanThresholdString;
            _settings.IsDefaultCleanThreshold = _originalSettings.IsDefaultCleanThreshold;
        }
    }
}
