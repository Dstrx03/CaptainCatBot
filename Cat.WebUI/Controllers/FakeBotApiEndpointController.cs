using System.Threading.Tasks;
using Cat.Application;

namespace Cat.WebUI.Controllers
{
    public class FakeBotApiEndpointController : BotApiEndpointControllerBase<FakeBotUpdate, Task>
    {
        public override async Task Update(FakeBotUpdate update) => await Mediator.Send(new FakeBotUpdateCommand(update));
    }
}
