using System;
using System.Threading.Tasks;
using Cat.Business.Services.InternalServices.Settings.SettingsModel;
using Cat.Domain.Entities.SystemValues;

namespace Cat.Business.Services.InternalServices.Settings
{
    public class RefresherSettingsManager : InternalServicesSettingsManager<RefresherSettings>
    {
        private string SettingsIntervalMinutesName => _internalServiceName + ".IntervalMinutes";
        private const int SettingsIntervalMinutesDefault = 15;

        public RefresherSettingsManager(ISystemValuesManager valuesManager) : base("Refresher", valuesManager)
        {
        }

        public override RefresherSettings GetSettings()
        {
            return new RefresherSettings
            {
                IsEnabled = GetIsEnabled(),
                IntervalMinutes = GetIntervalMinutes()
            };
        }

        public override async Task<RefresherSettings> GetSettingsAsync()
        {
            return new RefresherSettings
            {
                IsEnabled = await GetIsEnabledAsync(),
                IntervalMinutes = await GetIntervalMinutesAsync()
            };
        }

        public override RefresherSettings SaveSettings(RefresherSettings settings)
        {
            SetIsEnabled(settings.IsEnabled);
            SetIntervalMinutes(settings.IntervalMinutes);
            return settings;
        }

        public override async Task<RefresherSettings> SaveSettingsAsync(RefresherSettings settings)
        {
            await SetIsEnabledAsync(settings.IsEnabled);
            await SetIntervalMinutesAsync(settings.IntervalMinutes);
            return settings;
        }

        private int GetIntervalMinutes()
        {
            var intervalMinutes = _valuesManager.Get(SettingsIntervalMinutesName);
            if (intervalMinutes == null)
            {
                intervalMinutes = SettingsIntervalMinutesDefault;
                _valuesManager.Set(intervalMinutes, SettingsIntervalMinutesName, SystemValueType.Int);
            }
            return (int)intervalMinutes;
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

        private void SetIntervalMinutes(int intervalMinutes)
        {
            _valuesManager.Set(intervalMinutes, SettingsIntervalMinutesName, SystemValueType.Int);
        }

        private async Task SetIntervalMinutesAsync(int intervalMinutes)
        {
            await _valuesManager.SetAsync(intervalMinutes, SettingsIntervalMinutesName, SystemValueType.Int);
        }
    }
}
