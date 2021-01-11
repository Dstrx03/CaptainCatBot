using System;

namespace Cat.WebUI.BotApiEndpointRouting
{
    public class InvalidBotApiEndpointRoutingPathException : Exception
    {
        public InvalidBotApiEndpointRoutingPathException(string? message) : base(message)
        {
        }
    }
}
