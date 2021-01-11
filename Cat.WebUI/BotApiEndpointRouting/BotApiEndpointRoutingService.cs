﻿using Cat.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Cat.WebUI.BotApiEndpointRouting
{
    public class BotApiEndpointRoutingService
    {
        private readonly ILogger<BotApiEndpointRoutingService> _logger;
        private readonly BotApiEndpointRoutingPathFormatUtils _routingPathFormatUtils;

        private readonly ConcurrentDictionary<string, BotApiEndpointBase> _controllerPathsDictionary;
        private readonly ConcurrentDictionary<string, (string controllerPath, BotApiEndpointBase botApiEndpoint)> _endpointPathsDictionary;

        public BotApiEndpointRoutingService(ILogger<BotApiEndpointRoutingService> logger, BotApiEndpointRoutingPathFormatUtils routingPathFormatUtils)
        {
            _logger = logger;
            _routingPathFormatUtils = routingPathFormatUtils;

            _controllerPathsDictionary = new ConcurrentDictionary<string, BotApiEndpointBase>();
            _endpointPathsDictionary = new ConcurrentDictionary<string, (string controllerPath, BotApiEndpointBase botApiEndpoint)>();
        }

        public bool IsControllerPath(string pathNormalized) =>
            _controllerPathsDictionary.ContainsKey(pathNormalized);

        public bool IsEndpointPath(string pathNormalized, out string controllerPath)
        {
            var isEndpointPath = _endpointPathsDictionary.TryGetValue(pathNormalized, out var value) &&
                                 BotApiComponentState.IsRegistered(value.botApiEndpoint);
            controllerPath = value.controllerPath;
            return isEndpointPath;
        }

        public void Update(BotApiEndpointBase botApiEndpoint)
        {
            CheckBotApiEndpointRoutingPathsConsistency(botApiEndpoint);
            CheckBotApiEndpointRoutingPathsIsNotPresentedByOtherEndpoint(botApiEndpoint);
            RemoveBotApiEndpointRoutingPaths(botApiEndpoint);
            AddBotApiEndpointRoutingPaths(botApiEndpoint);
        }

        private void AddBotApiEndpointRoutingPaths(BotApiEndpointBase botApiEndpoint)
        {
            TryAddToPathsDictionary(_controllerPathsDictionary, botApiEndpoint.RoutingPaths.Select(_ => (_.ControllerPathNormalized, botApiEndpoint)), botApiEndpoint);
            TryAddToPathsDictionary(_endpointPathsDictionary, botApiEndpoint.RoutingPaths.Select(_ => (_.EndpointPathNormalized, (_.ControllerPath, botApiEndpoint))), botApiEndpoint);
        }

        private void RemoveBotApiEndpointRoutingPaths(BotApiEndpointBase botApiEndpoint)
        {
            TryRemoveFromPathsDictionary(_controllerPathsDictionary, _controllerPathsDictionary.Where(_ => _.Value == botApiEndpoint).Select(_ => _.Key), botApiEndpoint);
            TryRemoveFromPathsDictionary(_endpointPathsDictionary, _endpointPathsDictionary.Where(_ => _.Value.botApiEndpoint == botApiEndpoint).Select(_ => _.Key), botApiEndpoint);
        }

        private void TryAddToPathsDictionary<TValue>(ConcurrentDictionary<string, TValue> concurrentDictionary,
            IEnumerable<(string key, TValue value)> keysValues, BotApiComponentBase botApiEndpoint)
        {
            var processedKeysSet = new HashSet<string>();
            foreach (var (key, value) in keysValues)
                if (processedKeysSet.Add(key) && !concurrentDictionary.TryAdd(key, value))
                    _logger.LogWarning($"Failed to add {botApiEndpoint.GetType().Name}'s path ({key}) to the {GetPathsDictionaryParameterName(concurrentDictionary)}, the element is already present.");
        }

        private void TryRemoveFromPathsDictionary<TValue>(ConcurrentDictionary<string, TValue> concurrentDictionary,
            IEnumerable<string> keys, BotApiComponentBase botApiEndpoint)
        {
            foreach (var key in keys)
                if (!concurrentDictionary.TryRemove(key, out _))
                    _logger.LogWarning($"Failed to remove {botApiEndpoint.GetType().Name}'s path ({key}) from the {GetPathsDictionaryParameterName(concurrentDictionary)}, the element is not found in the set.");
        }

        private void CheckBotApiEndpointRoutingPathsConsistency(BotApiEndpointBase botApiEndpoint)
        {
            if (botApiEndpoint == null)
                throw new ArgumentNullException(nameof(botApiEndpoint));
            if (botApiEndpoint.RoutingPaths == null || !botApiEndpoint.RoutingPaths.Any())
                throw new InvalidBotApiEndpointException($"{botApiEndpoint.GetType().Name}'s Routing Paths collection cannot be null or empty.");

            var processedEndpointPathsSet = new HashSet<string>();
            var processedControllerPathsSet = new HashSet<string>();
            foreach (var routingPath in botApiEndpoint.RoutingPaths)
            {
                CheckRoutingPathValuesFormat(routingPath, botApiEndpoint);
                CheckEndpointPathsIsDistinct(routingPath, processedEndpointPathsSet, botApiEndpoint);
                CheckControllerPathIsConsumable(routingPath, processedControllerPathsSet, botApiEndpoint);
            }
        }

        private void CheckRoutingPathValuesFormat(BotApiEndpointRoutingPath routingPath, BotApiEndpointBase botApiEndpoint)
        {
            if (!RoutingPathValuesAreNotNull(routingPath))
                throw new InvalidBotApiEndpointRoutingPathException($"{botApiEndpoint.GetType().Name}'s Routing Path cannot have values that is null ({routingPath.Details()}).");

            if (!_routingPathFormatUtils.ControllerPathIsInCorrectFormat(routingPath.ControllerPath))
                throw new InvalidBotApiEndpointRoutingPathFormatException($"{botApiEndpoint.GetType().Name}'s Controller Path is not in a correct format ({routingPath.ControllerPath}).");
            if (!_routingPathFormatUtils.EndpointPathIsInCorrectFormat(routingPath.EndpointPath))
                throw new InvalidBotApiEndpointRoutingPathFormatException($"{botApiEndpoint.GetType().Name}'s Endpoint Path is not in a correct format ({routingPath.EndpointPath}).");

            if (!RoutingPathValuesAreNormalized(routingPath))
                throw new InvalidBotApiEndpointRoutingPathFormatException($"{botApiEndpoint.GetType().Name}'s Routing Path have values that is not normalized ({routingPath.Details()}).");
        }

        private void CheckEndpointPathsIsDistinct(BotApiEndpointRoutingPath routingPath, HashSet<string> processedEndpointPathsSet, BotApiEndpointBase botApiEndpoint)
        {
            var endpointPathsIsDistinct = processedEndpointPathsSet.Add(routingPath.EndpointPathNormalized);
            if (!endpointPathsIsDistinct)
                throw new InvalidBotApiEndpointRoutingPathException($"{botApiEndpoint.GetType().Name}'s Routing Paths collection cannot contain repeating Endpoint Paths ({routingPath.EndpointPath}).");
        }

        private void CheckControllerPathIsConsumable(BotApiEndpointRoutingPath routingPath, HashSet<string> processedControllerPathsSet, BotApiEndpointBase botApiEndpoint)
        {
            if (processedControllerPathsSet.Add(routingPath.ControllerPathNormalized) &&
                !_routingPathFormatUtils.ControllerPathMatchesConsumableControllerActionRoute(routingPath.ControllerPathNormalized))
                throw new InvalidBotApiEndpointRoutingPathException($"{botApiEndpoint.GetType().Name}'s Controller Path ({routingPath.ControllerPath}) do not match any consumable controller action route.");
        }

        private void CheckBotApiEndpointRoutingPathsIsNotPresentedByOtherEndpoint(BotApiEndpointBase botApiEndpoint)
        {
            var processedControllerPathsSet = new HashSet<string>();
            foreach (var routingPath in botApiEndpoint.RoutingPaths)
            {
                if (processedControllerPathsSet.Add(routingPath.ControllerPathNormalized) &&
                    ControllerPathIsAlreadyPresentedByOtherEndpoint(routingPath.ControllerPathNormalized, botApiEndpoint))
                    throw new InvalidBotApiEndpointRoutingPathException($"{botApiEndpoint.GetType().Name}'s Routing Paths collection cannot contain Controller Path ({routingPath.ControllerPath}) that is already presented by other endpoint.");

                if (EndpointPathIsAlreadyPresentedByOtherEndpoint(routingPath.EndpointPathNormalized, botApiEndpoint))
                    throw new InvalidBotApiEndpointRoutingPathException($"{botApiEndpoint.GetType().Name}'s Routing Paths collection cannot contain Endpoint Path ({routingPath.EndpointPath}) that is already presented by other endpoint.");
            }
        }

        private bool RoutingPathValuesAreNotNull(BotApiEndpointRoutingPath routingPath) =>
            routingPath.ControllerPath != null && routingPath.ControllerPathNormalized != null &&
            routingPath.EndpointPath != null && routingPath.EndpointPathNormalized != null;

        private bool RoutingPathValuesAreNormalized(BotApiEndpointRoutingPath routingPath) =>
            _routingPathFormatUtils.NormalizePath(routingPath.ControllerPath) == routingPath.ControllerPathNormalized &&
            _routingPathFormatUtils.NormalizePath(routingPath.EndpointPath) == routingPath.EndpointPathNormalized;

        private bool ControllerPathIsAlreadyPresentedByOtherEndpoint(string controllerPathNormalized, BotApiEndpointBase botApiEndpoint) =>
            _controllerPathsDictionary.TryGetValue(controllerPathNormalized, out var value) && value != botApiEndpoint;

        private bool EndpointPathIsAlreadyPresentedByOtherEndpoint(string endpointPathNormalized, BotApiEndpointBase botApiEndpoint) =>
            _endpointPathsDictionary.TryGetValue(endpointPathNormalized, out var value) && value.botApiEndpoint != botApiEndpoint;

        private string GetPathsDictionaryParameterName<TValue>(ConcurrentDictionary<string, TValue> concurrentDictionary)
        {
            if (_controllerPathsDictionary.Equals(concurrentDictionary))
                return nameof(_controllerPathsDictionary);
            if (_endpointPathsDictionary.Equals(concurrentDictionary))
                return nameof(_endpointPathsDictionary);
            return null;
        }
    }
}