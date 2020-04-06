using Cat.Common.Formatters;

namespace Cat.Common.AppSettings
{
    public interface IAppSettings
    {
        string AppTitle { get; }

        string AppVersion { get; }

        IAppTitleFormatter AppTitleFormatter { get; } 

        string ObligatoryAdminName { get; }

        string ObligatoryAdminPassword { get; }

        string ATriggerApiKey { get; }

        string ATriggerApiSecret { get; }

    }
}