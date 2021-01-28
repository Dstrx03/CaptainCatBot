using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Sender
{
    public interface IBotApiSender
    {
        Task SendMessageAsync(string message);
    }
}
