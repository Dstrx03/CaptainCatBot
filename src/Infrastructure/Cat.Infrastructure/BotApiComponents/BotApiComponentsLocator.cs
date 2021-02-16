using Cat.Domain.BotApiComponents.Component;
using Cat.Domain.BotApiComponents.ComponentsLocator;
using System.Collections.Generic;

namespace Cat.Infrastructure.BotApiComponents
{
    public class BotApiComponentsLocator : IBotApiComponentsLocator
    {
        public BotApiComponentsLocator(IEnumerable<IBotApiComponent> botApiComponents)
        {
            Components = botApiComponents;
        }

        public IEnumerable<IBotApiComponent> Components { get; }
    }
}
