using Cat.Domain.BotApiComponents.ComponentsLocator;
using Cat.Infrastructure.BotApiComponents;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBotApiComponentsLocator, BotApiComponentsLocator>(); // todo: consider more appropriate lifetime?

            return services;
        }
    }
}
