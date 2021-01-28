using Cat.Presentation.BotApiEndpointRouting.BotApiComponents;
using Cat.Presentation.Web.BotApiComponents;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddPresentationBotApiEndpointRouting();

            services.AddBotApiEndpoint<FakeBotApiEndpoint, FakeBotApiEndpoint.Factory>();

            return services;
        }
    }
}
