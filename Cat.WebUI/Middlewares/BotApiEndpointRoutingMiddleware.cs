using System.Threading.Tasks;
using Cat.WebUI.BotApiEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cat.WebUI.Middlewares
{
    public class BotApiEndpointRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BotApiEndpointRoutingService _botApiEndpointRoutingService;

        public BotApiEndpointRoutingMiddleware(RequestDelegate next, BotApiEndpointRoutingService botApiEndpointRoutingService)
        {
            _next = next;
            _botApiEndpointRoutingService = botApiEndpointRoutingService;
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
