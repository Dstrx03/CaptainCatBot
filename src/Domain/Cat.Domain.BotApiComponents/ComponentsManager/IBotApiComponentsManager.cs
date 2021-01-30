using Cat.Domain.BotApiComponents.Component;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.ComponentsManager
{
    public interface IBotApiComponentsManager : IBotApiComponent
    {
        Task RegisterComponentsAsync();
        Task UnregisterComponentsAsync();
        bool RegisterAtApplicationStart { get; }
        bool RegisterAtApplicationStartAfterAppHost => false;
    }
}
