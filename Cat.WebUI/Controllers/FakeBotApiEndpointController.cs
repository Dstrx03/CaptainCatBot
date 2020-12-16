using System.Threading.Tasks;
using Cat.Application;

namespace Cat.WebUI.Controllers
{
    public class FakeBotApiEndpointController : BotApiEndpointControllerBase<FakeBotUpdate, Task>
    {
        public override Task Update(FakeBotUpdate update) => Mediator.Send(new FakeBotUpdateCommand(update));
    }
}
