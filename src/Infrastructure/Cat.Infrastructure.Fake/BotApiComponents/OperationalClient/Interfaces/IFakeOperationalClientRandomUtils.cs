using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using System;
using System.Collections.Generic;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces
{
    public interface IFakeOperationalClientRandomUtils
    {
        IEnumerable<FakeBotUpdate> NextUpdates();
        TimeSpan NextTimeout();
        bool NextBoolean(int difficultyClass);
    }
}
