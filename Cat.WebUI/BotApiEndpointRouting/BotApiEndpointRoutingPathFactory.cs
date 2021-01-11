﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Cat.WebUI.BotApiEndpointRouting
{
    public class BotApiEndpointRoutingPathFactory
    {
        private readonly BotApiEndpointRoutingPathFormatUtils _routingPathFormatUtils;

        public BotApiEndpointRoutingPathFactory(BotApiEndpointRoutingPathFormatUtils routingPathFormatUtils)
        {
            _routingPathFormatUtils = routingPathFormatUtils;
        }

        public BotApiEndpointRoutingPath[] Create(IEnumerable<(string controllerPathTemplate, string controllerName, string endpointPath)> pathItemsCollection, BotApiEndpointBase botApiEndpoint)
        {
            if (botApiEndpoint == null)
                throw new ArgumentNullException(nameof(botApiEndpoint));

            var pathItemsList = pathItemsCollection?.ToList();

            // todo: consider to make Bot API Component names (Fake, Telegram, etc.) easy to maintain in messages - messages feature to generate component name etc. based on the type of the component
            if (pathItemsList == null || !pathItemsList.Any())
                throw new ArgumentException($"{botApiEndpoint.GetType().Name}'s Path Items collection cannot be null or empty.", nameof(pathItemsCollection));

            var routingPathsArray = new BotApiEndpointRoutingPath[pathItemsList.Count];
            for (var i = 0; i < pathItemsList.Count; i++)
            {
                routingPathsArray[i] = CreateRoutingPath(pathItemsList[i], botApiEndpoint);
            }

            return routingPathsArray;
        }

        private BotApiEndpointRoutingPath CreateRoutingPath((string controllerPathTemplate, string controllerName, string endpointPath) pathItems, BotApiEndpointBase botApiEndpoint)
        {
            CheckPathItems(pathItems, botApiEndpoint);

            var controllerPath = _routingPathFormatUtils.ParseControllerPathTemplate(pathItems.controllerPathTemplate, pathItems.controllerName);
            var endpointPath = pathItems.endpointPath;

            return new BotApiEndpointRoutingPath
            {
                ControllerPath = controllerPath,
                ControllerPathNormalized = _routingPathFormatUtils.NormalizePath(controllerPath),
                EndpointPath = endpointPath,
                EndpointPathNormalized = _routingPathFormatUtils.NormalizePath(endpointPath)
            };
        }

        private void CheckPathItems((string controllerPathTemplate, string controllerName, string endpointPath) pathItems, BotApiEndpointBase botApiEndpoint)
        {
            var (controllerPathTemplate, controllerName, endpointPath) = pathItems;

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