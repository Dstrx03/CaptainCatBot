using System;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Exceptions
{
    public class FakeOperationalClientEmulatedException : Exception
    {
        private const string DefaultMessage = "Operation is suspended due to the emulation of a Fake Operational Client malfunction.";

        public FakeOperationalClientEmulatedException() : base(DefaultMessage)
        {
        }

        public FakeOperationalClientEmulatedException(string message) : base(message)
        {
        }

        public FakeOperationalClientEmulatedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
