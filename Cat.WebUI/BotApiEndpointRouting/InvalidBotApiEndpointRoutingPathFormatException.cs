using System;

namespace Cat.WebUI.BotApiEndpointRouting
{
    public class InvalidBotApiEndpointRoutingPathFormatException : Exception
    {
        public InvalidBotApiEndpointRoutingPathFormatException(string? message) : base(message)
        {
        }
    }
}
