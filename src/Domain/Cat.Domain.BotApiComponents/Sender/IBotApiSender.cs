using Cat.Domain.BotApiComponents.Component;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Sender
{
    public interface IBotApiSender : IBotApiComponent
    {
        Task SendMessageAsync(string message);
    }
}
