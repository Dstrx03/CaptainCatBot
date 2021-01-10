using Cat.WebUI.BotApiEndpointRouting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Cat.WebUI.Middlewares
{
    public class BotApiEndpointRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BotApiEndpointRoutingService _routingService;
        private readonly BotApiEndpointRoutingPathFormatUtils _routingPathFormatUtils;

        public BotApiEndpointRoutingMiddleware(RequestDelegate next, BotApiEndpointRoutingService routingService, BotApiEndpointRoutingPathFormatUtils routingPathFormatUtils)
        {
            _next = next;
            _routingService = routingService;
            _routingPathFormatUtils = routingPathFormatUtils;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var pathNormalized = _routingPathFormatUtils.GetNormalizedRequestPath(context);

            if (_routingService.IsControllerPath(pathNormalized))
            {
                context.Response.StatusCode = 405; // todo: graceful handling
                return;
            }

            if (_routingService.IsEndpointPath(pathNormalized, out var controllerPath))
            {
                context.Request.Path = controllerPath;
            }

            await _next(context);
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
