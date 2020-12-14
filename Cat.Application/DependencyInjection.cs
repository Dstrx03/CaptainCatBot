using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System.Reflection;

namespace Cat.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddBotUpdatePipelineBehavior();

            return services;
        }

        private static IServiceCollection AddBotUpdatePipelineBehavior(this IServiceCollection services)
        {
            Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IBotUpdateCommand<>)) && !type.IsAbstract && !type.IsInterface)
                .ToList()
                .ForEach(botUpdateCommandType =>
                {
                    var botUpdateType = botUpdateCommandType.GetInterfaces()
                        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBotUpdateCommand<>))
                        .GenericTypeArguments.Single();

                    var serviceType = typeof(IPipelineBehavior<,>).MakeGenericType(botUpdateCommandType, typeof(Unit));
                    var implementationType = typeof(BotUpdatePipelineBehavior<,>).MakeGenericType(botUpdateCommandType, botUpdateType);

                    services.TryAddTransient(serviceType, implementationType);
                });

            return services;
        }
    }
}
