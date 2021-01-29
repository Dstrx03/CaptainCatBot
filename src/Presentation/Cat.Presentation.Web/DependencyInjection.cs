using Cat.Presentation.BotApiEndpointRouting.BotApiComponents;
using Cat.Presentation.Web.BotApiComponents;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationWeb(this IServiceCollection services)
        {
            services.AddBotApiEndpoint<FakeBotApiEndpoint, FakeBotApiEndpoint.Factory>();

            return services;
        }
    }
}
