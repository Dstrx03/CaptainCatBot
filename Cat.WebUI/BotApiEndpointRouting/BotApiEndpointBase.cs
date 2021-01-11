﻿using Cat.Domain;
using Cat.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cat.WebUI.BotApiEndpointRouting
{
    public abstract class BotApiEndpointBase : BotApiComponentBase, IBotApiEndpoint
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BotApiEndpointRoutingService _routingService;

        protected BotApiEndpointBase(IServiceProvider serviceProvider, BotApiEndpointRoutingService routingService)
        {
            _serviceProvider = serviceProvider;
            _routingService = routingService;
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

        protected void SetRoutingPaths(IEnumerable<(string controllerPathTemplate, string controllerName, string endpointPath)> pathItemsCollection)
        {
            var originalRoutingPaths = RoutingPaths;
            try
            {
                ApplyRoutingPaths(pathItemsCollection);
            }
            catch (Exception)
            {
                RoutingPaths = originalRoutingPaths;
                throw;
            }
        }

        private void ApplyRoutingPaths(IEnumerable<(string controllerPathTemplate, string controllerName, string endpointPath)> pathItemsCollection)
        {
            using var scope = _serviceProvider.CreateScope();
            var routingPathFactory = scope.ServiceProvider.GetRequiredService<BotApiEndpointRoutingPathFactory>();
            RoutingPaths = routingPathFactory.Create(pathItemsCollection, this);
            _routingService.Update(this);
        }

        public abstract class FactoryBase<TBotApiEndpoint> where TBotApiEndpoint : BotApiEndpointBase
        {
            public TBotApiEndpoint Create(IServiceProvider serviceProvider)
            {
                var instance = CreateInstance(serviceProvider);
                CheckCreatedInstance(instance);
                return instance;
            }

            protected abstract TBotApiEndpoint CreateInstance(IServiceProvider serviceProvider);

            private void CheckCreatedInstance(TBotApiEndpoint instance)
            {
                if (instance == null)
                    throw new InvalidBotApiEndpointException($"{typeof(TBotApiEndpoint).Name} instance was not provided by the responsible factory.");
                if (instance.RoutingPaths == null || !instance.RoutingPaths.Any())
                    throw new InvalidBotApiEndpointException($"{typeof(TBotApiEndpoint).Name}'s Routing Paths was not correctly set during the creation of the instance (Routing Paths collection is {instance.RoutingPaths.BarEnumerable()}), please use {nameof(instance.SetRoutingPaths)} method to set Routing Paths.");
            }
        }
    }
}
