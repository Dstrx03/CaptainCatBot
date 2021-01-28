using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using System.Threading.Tasks;

namespace Cat.Presentation.Web.Controllers.BotApiEndpointRouting
{
    public class FakeBotApiEndpointController : BotApiEndpointControllerBase<FakeBotUpdate, Task>
    {
        public override Task Update(FakeBotUpdate update) => Mediator.Send(new FakeBotUpdateCommand(update));
    }
}
