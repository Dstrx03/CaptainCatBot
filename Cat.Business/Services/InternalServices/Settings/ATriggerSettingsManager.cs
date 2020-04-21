
using System.Threading.Tasks;
using Cat.Business.Services.InternalServices.Settings.SettingsModel;
using Cat.Domain.Entities.SystemValues;

namespace Cat.Business.Services.InternalServices.Settings
{
    public class ATriggerSettingsManager : InternalServicesSettingsManager<ATriggerSettings>
    {
        private string SettingsTimeSliceMinutesName => _internalServiceName + ".TimeSliceMinutes";
        private const int SettingsTimeSliceMinutesDefault = 1;

        public ATriggerSettingsManager(ISystemValuesManager valuesManager) : base("ATrigger", valuesManager)
        {
        }

        public override ATriggerSettings GetSettings()
        {
            return new ATriggerSettings
            {
                IsEnabled = GetIsEnabled(),
                TimeSliceMinutes = GetTimeSliceMinutes()
            };
        }

        public override async Task<ATriggerSettings> GetSettingsAsync()
        {
            return new ATriggerSettings
            {
                IsEnabled = await GetIsEnabledAsync(),
                TimeSliceMinutes = await GetTimeSliceMinutesAsync()
            };
        }

        public override ATriggerSettings SaveSettings(ATriggerSettings settings)
        {
            SetIsEnabled(settings.IsEnabled);
            SetTimeSliceMinutes(settings.TimeSliceMinutes);
            return settings;
        }

        public override async Task<ATriggerSettings> SaveSettingsAsync(ATriggerSettings settings)
        {
            await SetIsEnabledAsync(settings.IsEnabled);
            await SetTimeSliceMinutesAsync(settings.TimeSliceMinutes);
            return settings;
        }

        private int GetTimeSliceMinutes()
        {
            var timeSliceMinutes = _valuesManager.Get(SettingsTimeSliceMinutesName);
            if (timeSliceMinutes == null)
            {
                timeSliceMinutes = SettingsTimeSliceMinutesDefault;
                _valuesManager.Set(timeSliceMinutes, SettingsTimeSliceMinutesName, SystemValueType.Int);
            }
            return (int)timeSliceMinutes;
        }

        private async Task<int> GetTimeSliceMinutesAsync()
        {
            var timeSliceMinutes = await _valuesManager.GetAsync(SettingsTimeSliceMinutesName);
            if (timeSliceMinutes == null)
            {
                timeSliceMinutes = SettingsTimeSliceMinutesDefault;
                await _valuesManager.SetAsync(timeSliceMinutes, SettingsTimeSliceMinutesName, SystemValueType.Int);
            }
            return (int)timeSliceMinutes;
        }

        private void SetTimeSliceMinutes(int timeSliceMinutes)
        {
            _valuesManager.Set(timeSliceMinutes, SettingsTimeSliceMinutesName, SystemValueType.Int);
        }

        private async Task SetTimeSliceMinutesAsync(int timeSliceMinutes)
        {
            await _valuesManager.SetAsync(timeSliceMinutes, SettingsTimeSliceMinutesName, SystemValueType.Int);
        }
    }
}
