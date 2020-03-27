using System.Web.Configuration;
using Cat.Web.App_Start;
using Telegram.AppSettings;

namespace Cat.Web.Infrastructure.Platform
{
    public class AppSettings : IAppSettings
    {

        public static IAppSettings Instance
        {
            get
            {
                var _container = StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer();
                return _container.GetInstance<IAppSettings>();
            }
        }

        public static ITelegramAppSettings InstanceTelegram
        {
            get
            {
                var _container = StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer();
                return _container.GetInstance<ITelegramAppSettings>();
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

        

    }
}