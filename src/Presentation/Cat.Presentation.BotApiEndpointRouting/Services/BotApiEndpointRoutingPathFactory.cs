using Cat.Presentation.BotApiEndpointRouting.BotApiComponents;
using Cat.Presentation.BotApiEndpointRouting.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cat.Presentation.BotApiEndpointRouting.Services
{
    public class BotApiEndpointRoutingPathFactory
    {
        private readonly BotApiEndpointRoutingPathFormatUtils _routingPathFormatUtils;

        public BotApiEndpointRoutingPathFactory(BotApiEndpointRoutingPathFormatUtils routingPathFormatUtils)
        {
            _routingPathFormatUtils = routingPathFormatUtils;
        }

        public BotApiEndpointRoutingPath[] Create(IEnumerable<(string controllerPathTemplate, string controllerName, string endpointPath, bool isWebhookUrlPath)> pathItemsCollection, BotApiEndpointBase botApiEndpoint)
        {
            if (botApiEndpoint == null)
                throw new ArgumentNullException(nameof(botApiEndpoint));

            var pathItemsList = pathItemsCollection?.ToList();

            if (pathItemsList == null || !pathItemsList.Any())
                throw new ArgumentException($"{botApiEndpoint.GetType().Name}'s Path Items collection cannot be null or empty.", nameof(pathItemsCollection));

            var routingPathsArray = new BotApiEndpointRoutingPath[pathItemsList.Count];
            for (var i = 0; i < pathItemsList.Count; i++)
            {
                routingPathsArray[i] = CreateRoutingPath(pathItemsList[i], botApiEndpoint);
            }

            return routingPathsArray;
        }

        private BotApiEndpointRoutingPath CreateRoutingPath((string controllerPathTemplate, string controllerName, string endpointPath, bool isWebhookUrlPath) pathItems, BotApiEndpointBase botApiEndpoint)
        {
            CheckPathItems(pathItems, botApiEndpoint);

            var controllerPath = _routingPathFormatUtils.ParseControllerPathTemplate(pathItems.controllerPathTemplate, pathItems.controllerName);
            var endpointPath = pathItems.endpointPath;
            var isWebhookUrlPath = pathItems.isWebhookUrlPath;

            return new BotApiEndpointRoutingPath
            {
                ControllerPath = controllerPath,
                ControllerPathNormalized = _routingPathFormatUtils.NormalizePath(controllerPath),
                EndpointPath = endpointPath,
                EndpointPathNormalized = _routingPathFormatUtils.NormalizePath(endpointPath),
                IsWebhookUrlPath = isWebhookUrlPath
            };
        }

        private void CheckPathItems((string controllerPathTemplate, string controllerName, string endpointPath, bool isWebhookUrlPath) pathItems, BotApiEndpointBase botApiEndpoint)
        {
            var (controllerPathTemplate, controllerName, endpointPath, isWebhookUrlPath) = pathItems;

            if (!PathItemIsNotNullEmptyOrWhitespace(controllerPathTemplate))
                throw new ArgumentException($"{botApiEndpoint.GetType().Name}'s Path Item ({nameof(controllerPathTemplate)}) cannot be null, empty or whitespace.");

            if (!PathItemIsNotNullEmptyOrWhitespace(controllerName))
                throw new ArgumentException($"{botApiEndpoint.GetType().Name}'s Path Item ({nameof(controllerName)}) cannot be null, empty or whitespace.");

            if (!PathItemIsNotNullEmptyOrWhitespace(endpointPath))
                throw new ArgumentException($"{botApiEndpoint.GetType().Name}'s Path Item ({nameof(endpointPath)}) cannot be null, empty or whitespace.");
        }

        private bool PathItemIsNotNullEmptyOrWhitespace(string path) =>
            !string.IsNullOrEmpty(path) && !string.IsNullOrWhiteSpace(path);
    }
}
