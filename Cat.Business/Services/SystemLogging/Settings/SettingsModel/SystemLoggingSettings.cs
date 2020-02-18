using System;

namespace Cat.Business.Services.SystemLogging.Settings.SettingsModel
{
    public class SystemLoggingSettings
    {
        public string Name { get; set; }

        public string Descriptor { get; set; }

        public TimeSpan? CleanThreshold { get; set; }

        public string CleanThresholdString { get; set; }

        public bool IsDefaultCleanThreshold { get; set; }
    }
}
