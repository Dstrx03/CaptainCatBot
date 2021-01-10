using Cat.Application;
using System.Threading.Tasks;

namespace Cat.WebUI.Controllers
{
    public class FakeBotApiEndpointController : BotApiEndpointControllerBase<FakeBotUpdate, Task>
    {
        public override Task Update(FakeBotUpdate update) => Mediator.Send(new FakeBotUpdateCommand(update));
    }
}
