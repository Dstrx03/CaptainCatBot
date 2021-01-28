using Cat.Presentation.BotApiEndpointRouting.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationBotApiEndpointRouting(this IServiceCollection services)
        {
            services.AddSingleton<BotApiEndpointRoutingService>();
            services.AddSingleton<BotApiEndpointRoutingPathFormatUtils>();
            services.AddTransient<BotApiEndpointRoutingPathFactory>();

            return services;
        }
    }
}
