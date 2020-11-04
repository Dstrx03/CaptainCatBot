using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Cat.Domain;

namespace Cat.WebUI.BotApiEndpoints
{
    public class BotApiEndpointRoutingService
    {
        private readonly ConcurrentDictionary<string, BotApiEndpointBase> _controllerPathsDictionary;
        private readonly ConcurrentDictionary<string, (string controllerPath, BotApiEndpointBase botApiEndpoint)> _endpointPathsDictionary;

        public BotApiEndpointRoutingService()
        {
            _controllerPathsDictionary = new ConcurrentDictionary<string, BotApiEndpointBase>();
            _endpointPathsDictionary = new ConcurrentDictionary<string, (string controllerPath, BotApiEndpointBase botApiEndpoint)>();
        }

        public bool IsControllerPath(string pathNormalized) => _controllerPathsDictionary.ContainsKey(pathNormalized);

        public bool IsEndpointPath(string pathNormalized, out string controllerPath)
        {
            var result = _endpointPathsDictionary.TryGetValue(pathNormalized, out var value) && BotApiComponentState.IsRegistered(value.botApiEndpoint);
            controllerPath = value.controllerPath;
            return result;
        }

        public bool ContainsRoutingPath(BotApiEndpointRoutingPath routingPath, BotApiEndpointBase botApiEndpoint) =>
            _controllerPathsDictionary.TryGetValue(routingPath.ControllerPathNormalized, out var valueCpd) && valueCpd != botApiEndpoint ||
            _endpointPathsDictionary.TryGetValue(routingPath.EndpointPathNormalized, out var valueEpd) && valueEpd.botApiEndpoint != botApiEndpoint;

        public bool ContainsRoutingPaths(IEnumerable<BotApiEndpointRoutingPath> routingPaths, BotApiEndpointBase botApiEndpoint)
        {
            foreach(var routingPath in routingPaths)
                if (ContainsRoutingPath(routingPath, botApiEndpoint)) return true;
            return false;
        }

        public void Update(BotApiEndpointBase botApiEndpoint)
        {
            RemoveRoutingPaths(botApiEndpoint);
            AddRoutingPaths(botApiEndpoint);
        }

        private void AddRoutingPaths(BotApiEndpointBase botApiEndpoint)
        {
            foreach (var routingPath in botApiEndpoint.RoutingPaths)
            {
                _controllerPathsDictionary.TryAdd(routingPath.ControllerPathNormalized, botApiEndpoint);
                _endpointPathsDictionary.TryAdd(routingPath.EndpointPathNormalized, (routingPath.ControllerPath, botApiEndpoint));
            }
        }

        private void RemoveRoutingPaths(BotApiEndpointBase botApiEndpoint)
        {
            foreach (var toRemove in _controllerPathsDictionary.Where(_ => _.Value == botApiEndpoint)) 
                _controllerPathsDictionary.TryRemove(toRemove.Key, out _);
            foreach (var toRemove in _endpointPathsDictionary.Where(_ => _.Value.botApiEndpoint == botApiEndpoint)) 
                _endpointPathsDictionary.TryRemove(toRemove.Key, out _);
        }
    }
}
