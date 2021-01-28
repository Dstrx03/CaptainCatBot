using System;

namespace Cat.Presentation.BotApiEndpointRouting.Exceptions
{
    public class InvalidBotApiEndpointException : Exception
    {
        public InvalidBotApiEndpointException(string message) : base(message)
        {
        }
    }
}
