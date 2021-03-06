﻿namespace Cat.Presentation.BotApiEndpointRouting.ValueObjects
{
    public struct BotApiEndpointRoutingPath
    {
        public string ControllerPath { get; init; }
        public string ControllerPathNormalized { get; init; }
        public string EndpointPath { get; init; }
        public string EndpointPathNormalized { get; init; }
        public bool IsWebhookUrlPath { get; init; }
    }
}
