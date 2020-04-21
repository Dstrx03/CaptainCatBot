using System.Threading.Tasks;
using Cat.Business.Services.InternalServices.Settings.SettingsModel;
using Cat.Domain.Entities.SystemValues;

namespace Cat.Business.Services.InternalServices.Settings
{
    public abstract class InternalServicesSettingsManager<T> where T : InternalServicesSettings
    {
        protected readonly ISystemValuesManager _valuesManager;
        protected readonly string _internalServiceName;

        private string SettingsIsEnabledName => _internalServiceName + ".IsEnabled";
        private const bool SettingsIsEnabledDefault = false;

        public abstract T GetSettings();
        public abstract Task<T> GetSettingsAsync();
        public abstract T SaveSettings(T settings);
        public abstract Task<T> SaveSettingsAsync(T settings);

        protected InternalServicesSettingsManager(string internalServiceName, ISystemValuesManager valuesManager)
        {
            _internalServiceName = internalServiceName;
            _valuesManager = valuesManager;
        }

        protected bool GetIsEnabled()
        {
            var isEnabled = _valuesManager.Get(SettingsIsEnabledName);
            if (isEnabled == null)
            {
                isEnabled = SettingsIsEnabledDefault;
                _valuesManager.Set(isEnabled, SettingsIsEnabledName, SystemValueType.Boolean);
            }
            return (bool)isEnabled;
        }

        protected async Task<bool> GetIsEnabledAsync()
        {
            var isEnabled = await _valuesManager.GetAsync(SettingsIsEnabledName);
            if (isEnabled == null)
            {
                isEnabled = SettingsIsEnabledDefault;
                await _valuesManager.SetAsync(isEnabled, SettingsIsEnabledName, SystemValueType.Boolean);
            }
            return (bool)isEnabled;
        }

        protected void SetIsEnabled(bool isEnabled)
        {
            _valuesManager.Set(isEnabled, SettingsIsEnabledName, SystemValueType.Boolean);
        }

        protected async Task SetIsEnabledAsync(bool isEnabled)
        {
            await _valuesManager.SetAsync(isEnabled, SettingsIsEnabledName, SystemValueType.Boolean);
        }
    }
}
