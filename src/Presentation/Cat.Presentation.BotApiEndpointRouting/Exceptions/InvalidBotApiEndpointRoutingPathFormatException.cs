using System;

namespace Cat.Presentation.BotApiEndpointRouting.Exceptions
{
    public class InvalidBotApiEndpointRoutingPathFormatException : Exception
    {
        public InvalidBotApiEndpointRoutingPathFormatException(string message) : base(message)
        {
        }
    }
}
