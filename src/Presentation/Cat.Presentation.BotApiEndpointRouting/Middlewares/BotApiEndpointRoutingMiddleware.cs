using Cat.Presentation.BotApiEndpointRouting.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Cat.Presentation.BotApiEndpointRouting.Middlewares
{
    public class BotApiEndpointRoutingMiddleware
    {
        public const string IsControllerPathCtxItmKey = "__" + nameof(BotApiEndpointRoutingMiddleware) + "_IsControllerPath";

        private readonly RequestDelegate _next;
        private readonly BotApiEndpointRoutingService _routingService;
        private readonly BotApiEndpointRoutingPathFormatUtils _routingPathFormatUtils;

        public BotApiEndpointRoutingMiddleware(
            RequestDelegate next,
            BotApiEndpointRoutingService routingService,
            BotApiEndpointRoutingPathFormatUtils routingPathFormatUtils)
        {
            _next = next;
            _routingService = routingService;
            _routingPathFormatUtils = routingPathFormatUtils;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var pathNormalized = _routingPathFormatUtils.GetNormalizedRequestPath(context);

            if (_routingService.IsControllerPath(pathNormalized))
                context.Items[IsControllerPathCtxItmKey] = true;

            if (_routingService.IsEndpointPath(pathNormalized, out var controllerPath))
                context.Request.Path = controllerPath;

            return _next(context);
        }
    }

    public static class BotApiEndpointRoutingMiddlewareExtensions
    {
        public static IApplicationBuilder UseBotApiEndpointRouting(this IApplicationBuilder app, Action<IEndpointRouteBuilder> configure)
        {
            app.UseMiddleware<BotApiEndpointRoutingMiddleware>();

            app.UseWhen(
                context =>
                    !context.Items.TryGetValue(BotApiEndpointRoutingMiddleware.IsControllerPathCtxItmKey, out var isControllerPath) ||
                    !(bool)isControllerPath,
                appBuilder =>
                {
                    appBuilder.UseRouting();
                    appBuilder.UseEndpoints(configure);
                });

            return app;
        }
    }
}
