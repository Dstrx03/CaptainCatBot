using System;
using System.Web.Configuration;
using Cat.Common.AppSettings.Assemblies;
using Cat.Common.Formatters;
using log4net;

namespace Cat.Common.AppSettings
{
    public class AppSettings : IAppSettings
    {
        private static IAppSettings _instance;
        private static readonly object _lockInstance = new object();

        private static ITelegramAppSettings _instanceTelegram;
        private static readonly object _lockInstanceTelegram = new object();

        private static IAppTitleFormatter _appTitleFormatter;
        private static readonly object _lockAppTitleFormatter = new object();

        public static IAppSettings Instance
        {
            get
            { 
                return GetInstance(ref _instance, _lockInstance, () => new AppSettings());
            }
        }

        public static ITelegramAppSettings InstanceTelegram
        {
            get
            {
                return GetInstance(ref _instanceTelegram, _lockInstanceTelegram, () => new TelegramAppSettings());
            }
        }

        public string AppTitle
        {
            get
            {
                return WebConfigurationManager.AppSettings["AppTitle"];
            }
        }

        public string AppVersion
        {
            get
            {
                return WebConfigurationManager.AppSettings["AppVersion"];
            }
        }

        public IAppTitleFormatter AppTitleFormatter
        {
            get
            {
                return GetInstance(ref _appTitleFormatter, _lockAppTitleFormatter, () => new AppTitleFormatter());
            }
        }

        public string ObligatoryAdminName
        {
            get
            {
                return WebConfigurationManager.AppSettings["ObligatoryAdminName"];
            }
        }

        public string ObligatoryAdminPassword
        {
            get
            {
                return WebConfigurationManager.AppSettings["ObligatoryAdminPassword"];
            }
        }

        public string ATriggerApiKey
        {
            get
            {
                return WebConfigurationManager.AppSettings["ATriggerApiKey"];
            }
        }

        public string ATriggerApiSecret
        {
            get
            {
                return WebConfigurationManager.AppSettings["ATriggerApiSecret"];
            }
        }
        


        private static T GetInstance<T>(ref T instance, object lockInstance, Func<T> createInstance)
        {
            if (instance == null)
            {
                lock (lockInstance)
                {
                    if (instance == null)
                    {
                        instance = createInstance();
                    }
                }
            }
            return instance;
        }
    }
}