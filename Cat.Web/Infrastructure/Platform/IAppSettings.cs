
namespace Cat.Web.Infrastructure.Platform
{
    public interface IAppSettings
    {

        string AppTitle { get; }

        string ObligatoryAdminName { get; }

        string ObligatoryAdminPassword { get; }

        string ATriggerApiKey { get; }

        string ATriggerApiSecret { get; }

    }
}