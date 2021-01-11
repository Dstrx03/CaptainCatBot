using System;

namespace Cat.WebUI.BotApiEndpointRouting
{
    public class InvalidBotApiEndpointException : Exception
    {
        public InvalidBotApiEndpointException(string? message) : base(message)
        {
        }
    }
}
