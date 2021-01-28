using Cat.Domain;
using Cat.Presentation.BotApiEndpointRouting.BotApiComponents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cat.Presentation.BotApiEndpointRouting.Services
{
    public class BotApiEndpointRoutingInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<BotApiEndpointRoutingInitializerOptions> _options;

        public BotApiEndpointRoutingInitializer(IServiceProvider serviceProvider, IOptions<BotApiEndpointRoutingInitializerOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var routingService = InitializeBotApiEndpointRoutingService();
            InitializeAllBotApiEndpoints();

            if (_options.Value.CheckControllersCovered)
                CheckAllConsumableControllerActionRoutesHaveMatchingBotApiEndpoints(routingService);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private BotApiEndpointRoutingService InitializeBotApiEndpointRoutingService() =>
            _serviceProvider.GetRequiredService<BotApiEndpointRoutingService>();

        private void InitializeAllBotApiEndpoints()
        {
            _serviceProvider.GetRequiredService<IEnumerable<BotApiEndpointBase>>();
        }

        private void CheckAllConsumableControllerActionRoutesHaveMatchingBotApiEndpoints(BotApiEndpointRoutingService routingService)
        {
            var routes = routingService
                .GetConsumableControllerActionRoutesNotMatchingWithAnyBotApiEndpoint()
                .ToList();
            if (routes.Any())
                throw new InvalidOperationException($"Cannot locate Bot API Endpoint(s) matching with the consumable controller action route(s) ({routes.BarEnumerableToStrFmt()}).");
        }
    }

    public class BotApiEndpointRoutingInitializerOptions
    {
        public bool CheckControllersCovered { get; set; } = true;
    }

    public static class BotApiEndpointRoutingInitializerExtensions
    {
        public static IServiceCollection AddBotApiEndpointRoutingInitializer(this IServiceCollection services, Action<BotApiEndpointRoutingInitializerOptions> configuration = null)
        {
            services.AddHostedService<BotApiEndpointRoutingInitializer>();
            if (configuration != null)
                services.Configure(configuration);

            return services;
        }
    }
}
