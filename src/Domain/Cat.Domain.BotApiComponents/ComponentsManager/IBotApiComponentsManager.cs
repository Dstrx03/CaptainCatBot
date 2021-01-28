using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.ComponentsManager
{
    public interface IBotApiComponentsManager
    {
        Task RegisterComponentsAsync();
        Task UnregisterComponentsAsync();
        bool RegisterAtApplicationStart { get; }
        bool RegisterAtApplicationStartAfterAppHost => false;
    }
}
