using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cat.Domain;
using Cat.Infrastructure;
using Cat.WebUI.BotApiEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cat.WebUI.Middleware
{
    public class BotApiEndpointRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<BotApiEndpointBase> _botApiEndpoints;

        public BotApiEndpointRoutingMiddleware(RequestDelegate next, IEnumerable<BotApiEndpointBase> botApiEndpoints, 
            /*TODO: REMOVE!*/ FakeBotApiClient fakeClient, FakeBotApiWebhook fakeWebhook, FakeBotApiPoller fakePoller, ILogger<BotApiEndpointRoutingMiddleware> logger/*TODO: REMOVE!*/)
        {
            _next = next;
            _botApiEndpoints = botApiEndpoints;

            // todo: move registration of endpoints to IBotApiComponentsManager
            fakeClient.RegisterClientAsync().Wait();
            foreach (var botApiEndpoint in _botApiEndpoints)
            {
                botApiEndpoint.RegisterEndpoint();
            }
            fakeWebhook.RegisterWebhookAsync().Wait();
            fakePoller.RegisterPoller();

            logger.LogDebug($"client: {fakeClient.ComponentState.State} {fakeClient.ComponentState.Description}");
            logger.LogDebug($"endpoint: {_botApiEndpoints.First().ComponentState.State} {_botApiEndpoints.First().ComponentState.Description}");
            logger.LogDebug($"webhook: {fakeWebhook.ComponentState.State} {fakeWebhook.ComponentState.Description}");
            logger.LogDebug($"poller: {fakePoller.ComponentState.State} {fakePoller.ComponentState.Description}");
            // todo: ===========================================================
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var pathNormalized = GetNormalizedRequestPath(context);

            var matchesControllerRoute = _botApiEndpoints
                .SelectMany(x => x.Paths)
                .Any(x => x?.ControllerPathNormalized == pathNormalized);
            if (matchesControllerRoute)
            {
                context.Response.StatusCode = 405;
                return;
            }

            var endpointRoute = _botApiEndpoints
                .Where(BotApiComponentState.IsRegistered)
                .SelectMany(x => x.Paths)
                .FirstOrDefault(x => x?.EndpointPathNormalized == pathNormalized);
            if (endpointRoute.HasValue)
            {
                context.Request.Path = endpointRoute.Value.ControllerPath;
            }

            await _next(context);
        }

        private string GetNormalizedRequestPath(HttpContext context)
        {
            // todo: string operations optimization & overall BotApiEndpointRoutingMiddleware optimization
            var path = context.Request.Path.ToUriComponent().ToLower(); // todo: ToLowerInvariant()?
            return path.EndsWith('/') && path.Length > 1 ? path.Substring(0, path.Length - 1) : path;
        }
    }
}
