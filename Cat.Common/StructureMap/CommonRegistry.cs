using Cat.Common.AppSettings;
using Cat.Common.AppSettings.Assemblies;
using Cat.Common.Formatters;
using StructureMap.Configuration.DSL;

namespace Cat.Common.StructureMap
{
    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<IAppSettings>().Use(() => AppSettings.AppSettings.Instance);
            For<ITelegramAppSettings>().Use(() => AppSettings.AppSettings.InstanceTelegram);
            For<IAppTitleFormatter>().Use(() => AppSettings.AppSettings.Instance.AppTitleFormatter);
        }
    }
}
