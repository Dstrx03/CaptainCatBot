using System;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces
{
    public interface IFakeOperationalClientTimeout
    {
        TimeSpan Timeout { get; }
        DateTime LastReset { get; }
        TimeSpan CurrentDuration { get; }
        bool IsNotElapsed { get; }

        void Reset(TimeSpan timeout);
        void Reset();
    }
}
