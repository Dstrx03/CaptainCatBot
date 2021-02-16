using Cat.Domain.BotApiComponents.Component;
using Cat.Domain.BotApiComponents.ComponentsLocator;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.ComponentsManager
{
    public interface IBotApiComponentsManager : IBotApiComponent
    {
        Task RegisterComponentsAsync(IBotApiComponentsLocator botApiComponentsLocator);
        Task UnregisterComponentsAsync(IBotApiComponentsLocator botApiComponentsLocator);
        bool RegisterAtApplicationStart { get; }
        bool RegisterAtApplicationStartAfterAppHost => false;
    }
}
