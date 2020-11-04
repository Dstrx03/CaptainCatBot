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
                throw new ArgumentException("a1"); // todo: exception message

            var routingPathsArrayLength = pathsList.Count;
            var routingPathsArray = new BotApiEndpointRoutingPath[routingPathsArrayLength];
            for (var i = 0; i < routingPathsArrayLength; i++)
            {
                var (controllerPathTemplate, controllerName, endpointPath) = pathsList[i];
                routingPathsArray[i] = CreateRoutingPath(controllerPathTemplate, controllerName, endpointPath); ;
            }

            if (!routingPathsArray.IsDistinct() || _botApiEndpointRoutingService.ContainsRoutingPaths(routingPathsArray, this)) 
                throw new ArgumentException("a2"); // todo: exception message

            RoutingPaths = routingPathsArray; 
            _botApiEndpointRoutingService.Update(this);
        }

        private BotApiEndpointRoutingPath CreateRoutingPath(string controllerPathTemplate, string controllerName, string endpointPath)
        {
            if (string.IsNullOrEmpty(controllerPathTemplate) ||
                string.IsNullOrEmpty(controllerName) ||
                string.IsNullOrEmpty(endpointPath) ||
                string.IsNullOrWhiteSpace(controllerPathTemplate) ||
                string.IsNullOrWhiteSpace(controllerName) ||
                string.IsNullOrWhiteSpace(endpointPath))
                throw new ArgumentException("b1"); // todo: exception message
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
                    throw new Exception("c1"); // todo: exception type, message
                if (instance.RoutingPaths == null || !instance.RoutingPaths.Any())
                    throw new Exception("c2"); // todo: exception type, message
            }
        }
    }
}
