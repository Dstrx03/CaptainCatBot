using System.Threading.Tasks;

namespace Cat.Domain
{
    public interface IBotApiSender
    {
        Task SendMessageAsync(string message);
    }
}
