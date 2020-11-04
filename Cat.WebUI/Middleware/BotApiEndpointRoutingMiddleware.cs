using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat.Infrastructure;
using Cat.WebUI.BotApiEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cat.WebUI.Middleware
{
    public class BotApiEndpointRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BotApiEndpointRoutingService _botApiEndpointRoutingService;

        private readonly List<BotApiEndpointBase> _botApiEndpoints;

        public BotApiEndpointRoutingMiddleware(RequestDelegate next, BotApiEndpointRoutingService botApiEndpointRoutingService/*TODO: REMOVE!*/, IEnumerable<BotApiEndpointBase> botApiEndpoints, /*TODO: REMOVE!*/
            /*TODO: REMOVE!*/ FakeBotApiClient fakeClient, FakeBotApiWebhook fakeWebhook, FakeBotApiPoller fakePoller, ILogger<BotApiEndpointRoutingMiddleware> logger/*TODO: REMOVE!*/)
        {
            _next = next;
            _botApiEndpointRoutingService = botApiEndpointRoutingService;

            // todo: move registration of endpoints to IBotApiComponentsManager
            _botApiEndpoints = botApiEndpoints.ToList();
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

            if (_botApiEndpointRoutingService.IsControllerPath(pathNormalized))
            {
                context.Response.StatusCode = 405; // todo: graceful handling
                return;
            }

            if (_botApiEndpointRoutingService.IsEndpointPath(pathNormalized, out var controllerPath))
            {
                context.Request.Path = controllerPath;
            }

            await _next(context);
        }

        private string GetNormalizedRequestPath(HttpContext context)
        {
            var path = context.Request.Path.ToUriComponent().ToUpperInvariant();
            return path.EndsWith('/') && path.Length > 1 ? path.Substring(0, path.Length - 1) : path;
        }
    }

    public static class BotApiEndpointRoutingMiddlewareExtensions // todo: appropriate place and name for this class
    {
        public static IApplicationBuilder UseBotApiEndpointRouting(this IApplicationBuilder app)
        {
            app.UseMiddleware<BotApiEndpointRoutingMiddleware>();
            app.UseRouting();
            return app;
        }
    }
}
