using System.Collections.Generic;
using System.Linq;

namespace Cat.WebUI.Extensions.BotApiEndpointRoutingPath // todo: more valid namespace?
{
    public static class BotApiEndpointRoutingPathExtensions
    {
        public static bool IsDistinct(this BotApiEndpoints.BotApiEndpointRoutingPath[] routingPaths)
        {
            if (routingPaths == null || !routingPaths.Any()) return true;
            var diffChecker = new HashSet<string>();
            var controllerPathsDistinct = routingPaths.Select(_ => _.ControllerPathNormalized).All(diffChecker.Add);
            var endpointPathsDistinct = routingPaths.Select(_ => _.EndpointPathNormalized).All(diffChecker.Add);
            return controllerPathsDistinct && endpointPathsDistinct;
        }
    }
}
