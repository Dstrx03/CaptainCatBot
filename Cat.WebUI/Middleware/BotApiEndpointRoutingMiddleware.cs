using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat.WebUI.BotApiEndpoints;
using Microsoft.AspNetCore.Http;

namespace Cat.WebUI.Middleware
{
    public class BotApiEndpointRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<BotApiEndpointBase> _botApiEndpointsCollection;

        public BotApiEndpointRoutingMiddleware(RequestDelegate next, IEnumerable<BotApiEndpointBase> botApiEndpointsCollection)
        {
            _next = next;
            _botApiEndpointsCollection = botApiEndpointsCollection;

            // todo: move registration of endpoints to IBotApiComponentsManager
            foreach (var botApiEndpoint in _botApiEndpointsCollection)
            {
                botApiEndpoint.RegisterEndpoint();
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.ToUriComponent().ToLower();

            var matchesControllerRoute = _botApiEndpointsCollection.FirstOrDefault(x => x.ControllerRoute == path);
            if (matchesControllerRoute != null)
            {
                context.Response.StatusCode = 405;
                return;
            }

            var matchesEndpointRoute = _botApiEndpointsCollection.FirstOrDefault(x => x.GetStatus() == true && x.EndpointRoute == path);
            if (matchesEndpointRoute != null)
            {
                context.Request.Path = matchesEndpointRoute.ControllerRoute;
            }

            await _next.Invoke(context);
        }
    }
}
