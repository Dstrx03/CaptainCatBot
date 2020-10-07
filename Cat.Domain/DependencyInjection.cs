using Microsoft.Extensions.DependencyInjection;

namespace Cat.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            // todo: use Lamar/StructureMap with default conventions, lifetimes etc.
            services.AddTransient<BotUpdateProcessor>();
            // todo: ==================================

            return services;
        }
    }
}
