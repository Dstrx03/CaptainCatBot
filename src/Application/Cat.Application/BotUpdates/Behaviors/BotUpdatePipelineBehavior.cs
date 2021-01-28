using Cat.Application.BotUpdates.Commands;
using Cat.Domain.BotUpdates.PreProcessor;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Cat.Application.BotUpdates.Behaviors
{
    public class BotUpdatePipelineBehavior<TRequest, TUpdate> : IPipelineBehavior<TRequest, Unit>
        where TRequest : IBotUpdateCommand<TUpdate>
    {
        private readonly IServiceProvider _serviceProvider;

        public BotUpdatePipelineBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            // todo: validation?

            var updatePreProcessor = (IBotUpdatePreProcessor<TUpdate>)_serviceProvider.GetService(typeof(IBotUpdatePreProcessor<TUpdate>));
            if (updatePreProcessor != null && updatePreProcessor.PreProcessingIsRequired(request.Update))
            {
                var shortCircuit = await updatePreProcessor.PreProcessUpdateAsync(request.Update);
                if (shortCircuit) return Unit.Value;
            }

            return await next();
        }
    }

    internal static class BotUpdatePipelineBehaviorExtensions
    {
        internal static IServiceCollection AddBotUpdatePipelineBehavior(this IServiceCollection services)
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
