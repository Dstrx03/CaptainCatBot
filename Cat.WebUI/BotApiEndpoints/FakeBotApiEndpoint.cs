using System;
using System.Text;
using Cat.WebUI.Controllers;
using Microsoft.Extensions.Logging;

namespace Cat.WebUI.BotApiEndpoints
{
    public class FakeBotApiEndpoint : BotApiEndpointBase
    {
        private readonly ILogger<FakeBotApiEndpoint> _logger;

        public FakeBotApiEndpoint(ILogger<FakeBotApiEndpoint> logger) : base(FakeBotApiEndpointController.PathTemplateUpdate, typeof(FakeBotApiEndpointController).Name)
        {
            _logger = logger;
            InitFakeEndpointRoute();
        }

        public override void RegisterEndpoint()
        {
            base.RegisterEndpoint();
            _logger.LogDebug("Fake Bot API Endpoint registered");
        }

        public override void UnregisterEndpoint()
        {
            base.UnregisterEndpoint();
            _logger.LogDebug("Fake Bot API Endpoint unregistered");
        }

        private void InitFakeEndpointRoute()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var builder = new StringBuilder("/FakeBotApiEndpoint_");

            var length = random.Next(8, 17);

            var offset = (int)'a';
            var lettersOffset = 26;

            for (var i = 0; i < length; i++)
            {
                builder.Append(random.Next(0, 2) == 0 ? ((char)random.Next(offset, offset + lettersOffset)).ToString() : random.Next(10).ToString());
            }

            EndpointPath = builder.ToString();
            _logger.LogDebug($"Fake Bot API Endpoint route: '{EndpointPath}'");
        }
    }
}
