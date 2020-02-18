using Cat.Business.Services.SystemLogging.Settings.SettingsModel;

namespace Cat.Business.Services.SystemLogging.Settings.SettingsBuilder
{
    public interface ISettingsBuilder
    {
        void Reset();

        SystemLoggingSettings GetResult();
    }

    public abstract class SettingsBuilder : ISettingsBuilder
    {
        protected SystemLoggingSettings _settings;

        protected abstract void BuildSettings();

        public void Reset()
        {
            _settings = new SystemLoggingSettings();
        }

        public SystemLoggingSettings GetResult()
        {
            Reset();
            BuildSettings();
            return _settings;
        }
    }
}
