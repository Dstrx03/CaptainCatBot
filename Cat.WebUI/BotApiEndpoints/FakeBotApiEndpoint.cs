using System;
using System.Collections.Generic;
using Cat.WebUI.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cat.WebUI.BotApiEndpoints
{
    public class FakeBotApiEndpoint : BotApiEndpointBase
    {
        private readonly ILogger<FakeBotApiEndpoint> _logger;

        private FakeBotApiEndpoint(BotApiEndpointRoutingService botApiEndpointRoutingService, ILogger<FakeBotApiEndpoint> logger) : base(botApiEndpointRoutingService)
        {
            _logger = logger;
            SetRoutingPaths(new List<(string, string, string)>
            {
                // todo: remove hardcoded EnpointPath value, it should be taken from specialized component with URL/path/route composition/management responsibility
                (FakeBotApiEndpointController.PathTemplateUpdate, typeof(FakeBotApiEndpointController).Name, "/FakeBotApiEndpoint")
            });
        }

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

        public class Factory : BotApiEndpointBase.FactoryBase<FakeBotApiEndpoint>
        {
            protected override FakeBotApiEndpoint CreateInstance(IServiceProvider serviceProvider)
            {
                var botApiEndpointRoutingService = serviceProvider.GetRequiredService<BotApiEndpointRoutingService>();
                var logger = serviceProvider.GetRequiredService<ILogger<FakeBotApiEndpoint>>();
                return new FakeBotApiEndpoint(botApiEndpointRoutingService, logger);
            }
        }
    }
}
