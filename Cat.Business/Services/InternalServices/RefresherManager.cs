using Cat.Domain.Entities.SystemValues;

namespace Cat.Business.Services.InternalServices
{
    public interface IRefresherManager
    {
        RefresherSettings GetSettings();
        RefresherSettings SaveSettings(RefresherSettings settings);
    }

    public class RefresherManager : IRefresherManager
    {
        private readonly ISystemValuesManager _valuesManager;

        public RefresherManager(ISystemValuesManager valuesManager)
        {
            _valuesManager = valuesManager;
        }

        public RefresherSettings GetSettings()
        {
            var isEnabled = _valuesManager.Get("Refresher.IsEnabled");
            if (isEnabled == null)
            {
                isEnabled = false;
                _valuesManager.Set(isEnabled, "Refresher.IsEnabled", SystemValueType.Boolean);
            }

            var intervalMinutes = _valuesManager.Get("Refresher.IntervalMinutes");
            if (intervalMinutes == null)
            {
                intervalMinutes = 15;
                _valuesManager.Set(intervalMinutes, "Refresher.IntervalMinutes", SystemValueType.Int);
            }

            return new RefresherSettings
            {
                IsEnabled = (bool)isEnabled, 
                IntervalMinutes = (int)intervalMinutes
            };
        }

        public RefresherSettings SaveSettings(RefresherSettings settings)
        {
            _valuesManager.Set(settings.IsEnabled, "Refresher.IsEnabled", SystemValueType.Boolean);
            _valuesManager.Set(settings.IntervalMinutes, "Refresher.IntervalMinutes", SystemValueType.Int);

            return settings;
        }
    }
}
