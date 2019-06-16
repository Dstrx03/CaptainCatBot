﻿using System.Web.Configuration;
using Cat.Web.App_Start;

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

        public string AppTitle {
            get
            {
                return WebConfigurationManager.AppSettings["AppTitle"];
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
    }
}