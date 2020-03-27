
namespace Cat.Web.Infrastructure.Platform
{
    public interface IAppSettings
    {

        string ObligatoryAdminName { get; }

        string ObligatoryAdminPassword { get; }

        string ATriggerApiKey { get; }

        string ATriggerApiSecret { get; }

    }
}