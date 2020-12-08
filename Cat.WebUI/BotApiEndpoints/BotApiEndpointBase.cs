using System;
using System.Collections.Generic;
using System.Linq;
using Cat.Domain;
using Cat.WebUI.Extensions.BotApiEndpointRoutingPath;

namespace Cat.WebUI.BotApiEndpoints
{
    public abstract class BotApiEndpointBase : BotApiComponentBase, IBotApiEndpoint
    {
        private readonly BotApiEndpointRoutingService _botApiEndpointRoutingService;

        protected BotApiEndpointBase(BotApiEndpointRoutingService botApiEndpointRoutingService)
        {
            _botApiEndpointRoutingService = botApiEndpointRoutingService;
        }

        public virtual void RegisterEndpoint()
        {
            ComponentState = BotApiComponentState.CreateRegistered();
        }

        public virtual void UnregisterEndpoint()
        {
            ComponentState = BotApiComponentState.CreateUnregistered();
        }

        public IEnumerable<BotApiEndpointRoutingPath> RoutingPaths { get; private set; }

        protected void SetRoutingPaths(IEnumerable<(string controllerPathTemplate, string controllerName, string endpointPath)> paths)
        {
            var pathsList = paths?.ToList();
            
            if (pathsList == null || !pathsList.Any())
                throw new ArgumentNullException(nameof(paths), $"{this.GetType().Name}'s Routing Paths collection cannot be null or empty.");

            var routingPathsArrayLength = pathsList.Count;
            var routingPathsArray = new BotApiEndpointRoutingPath[routingPathsArrayLength];
            for (var i = 0; i < routingPathsArrayLength; i++)
            {
                var (controllerPathTemplate, controllerName, endpointPath) = pathsList[i];
                routingPathsArray[i] = CreateRoutingPath(controllerPathTemplate, controllerName, endpointPath);
            }
            
            if (!routingPathsArray.IsDistinct() || _botApiEndpointRoutingService.ContainsRoutingPaths(routingPathsArray, this))
                throw new InvalidOperationException($"{this.GetType().Name}'s Routing Paths collection cannot contain repeating values or values which are already presented in Bot API Endpoint Routing Service.");

            RoutingPaths = routingPathsArray; 
            _botApiEndpointRoutingService.Update(this);
        }

        private BotApiEndpointRoutingPath CreateRoutingPath(string controllerPathTemplate, string controllerName, string endpointPath)
        {
            if (string.IsNullOrEmpty(controllerPathTemplate) || string.IsNullOrWhiteSpace(controllerPathTemplate))
                throw new ArgumentNullException(nameof(controllerPathTemplate), $"{this.GetType().Name}'s Routing Path controller path template value cannot be null, empty or whitespace.");
            if (string.IsNullOrEmpty(controllerName) || string.IsNullOrWhiteSpace(controllerName))
                throw new ArgumentNullException(nameof(controllerName), $"{this.GetType().Name}'s Routing Path controller name value cannot be null, empty or whitespace.");
            if (string.IsNullOrEmpty(endpointPath) || string.IsNullOrWhiteSpace(endpointPath))
                throw new ArgumentNullException(nameof(endpointPath), $"{this.GetType().Name}'s Routing Path endpoint path value cannot be null, empty or whitespace.");

            return new BotApiEndpointRoutingPath(ParseControllerPath(controllerPathTemplate, controllerName), endpointPath);
        }

        private string ParseControllerPath(string controllerPathTemplate, string controllerName) =>
            controllerPathTemplate.Replace("[controller]", controllerName.Replace("Controller", string.Empty));

        public abstract class FactoryBase<T> where T : BotApiEndpointBase
        {
            public T Create(IServiceProvider serviceProvider)
            {
                var instance = CreateInstance(serviceProvider);
                CheckCreatedInstance(instance);
                return instance;
            }

            protected abstract T CreateInstance(IServiceProvider serviceProvider);

            private void CheckCreatedInstance(T instance)
            {
                if (instance == null)
                    throw new InvalidOperationException($"{typeof(T).Name}'s instance cannot be null.");
                if (instance.RoutingPaths == null || !instance.RoutingPaths.Any())
                    throw new InvalidOperationException($"The Routing Paths collection of {typeof(T).Name}'s instance cannot be null or empty.");
            }
        }
    }
}
