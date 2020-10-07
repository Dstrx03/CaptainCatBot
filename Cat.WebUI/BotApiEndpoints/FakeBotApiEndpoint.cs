using Cat.WebUI.Controllers;
using Microsoft.Extensions.Logging;

namespace Cat.WebUI.BotApiEndpoints
{
    public class FakeBotApiEndpoint : BotApiEndpointBase
    {
        private readonly ILogger<FakeBotApiEndpoint> _logger;

        public FakeBotApiEndpoint(ILogger<FakeBotApiEndpoint> logger)
        {
            _logger = logger;
            SetPaths(new[]
            {
                // todo: remove hardcoded EnpointPath value, it should be taken from specialized component with URL/path/route composition/management responsibility
                CreatePath(FakeBotApiEndpointController.PathTemplateUpdate, typeof(FakeBotApiEndpointController).Name, "/FakeBotApiEndpoint")
            });
        }

        public override void RegisterEndpoint()
        {
            base.RegisterEndpoint();
            _logger.LogDebug("TODO log registered"); // todo: apply single text format convention for all Fake Bot API components log messages
        }

        public override void UnregisterEndpoint()
        {
            base.UnregisterEndpoint();
            _logger.LogDebug("TODO log unregistered"); // todo: apply single text format convention for all Fake Bot API components log messages
        }
    }




    // todo: move to separate component with URL composition/management responsibility
    /*private void InitFakeEndpointRoute()
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
    }*/
}
