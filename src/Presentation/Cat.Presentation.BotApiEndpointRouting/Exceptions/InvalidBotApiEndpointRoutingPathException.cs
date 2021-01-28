using System;

namespace Cat.Presentation.BotApiEndpointRouting.Exceptions
{
    public class InvalidBotApiEndpointRoutingPathException : Exception
    {
        public InvalidBotApiEndpointRoutingPathException(string message) : base(message)
        {
        }
    }
}
