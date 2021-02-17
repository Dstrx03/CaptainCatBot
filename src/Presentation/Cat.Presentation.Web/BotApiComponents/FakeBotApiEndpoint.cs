using Cat.Domain.BotApiComponents.Component;
using Cat.Presentation.BotApiEndpointRouting.BotApiComponents;
using Cat.Presentation.BotApiEndpointRouting.Services;
using Cat.Presentation.Web.Controllers.BotApiEndpointRouting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Cat.Presentation.Web.BotApiComponents
{
    public class FakeBotApiEndpoint : BotApiEndpointBase
    {
        private readonly ILogger<FakeBotApiEndpoint> _logger;

        private FakeBotApiEndpoint(IServiceProvider serviceProvider, BotApiEndpointRoutingService routingService, ILogger<FakeBotApiEndpoint> logger) :
            base(serviceProvider, routingService)
        {
            _logger = logger;
            SetRoutingPaths(new[]
            {
                // todo: remove hardcoded EnpointPath value, it should be taken from specialized component with URL/path/route composition/management responsibility
                (FakeBotApiEndpointController.UpdatePathTemplate, nameof(FakeBotApiEndpointController), "/FakeBotApiEndpoint", true),
            });
        }

        public override BotApiComponentDescriptor ComponentDescriptor =>
            BotApiComponentDescriptor.Fake;

        public override void RegisterEndpoint()
        {
            base.RegisterEndpoint();
            _logger.LogInformation("Fake Bot API Endpoint registered.");
        }

        public override void UnregisterEndpoint()
        {
            base.UnregisterEndpoint();
            _logger.LogInformation("Fake Bot API Endpoint unregistered.");
        }

        public class Factory : FactoryBase<FakeBotApiEndpoint>
        {
            protected override FakeBotApiEndpoint CreateInstance(IServiceProvider serviceProvider)
            {
                var routingService = serviceProvider.GetRequiredService<BotApiEndpointRoutingService>();
                var logger = serviceProvider.GetRequiredService<ILogger<FakeBotApiEndpoint>>();
                return new FakeBotApiEndpoint(serviceProvider, routingService, logger);
            }
        }
    }
}
