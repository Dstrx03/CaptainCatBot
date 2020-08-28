using System.Threading.Tasks;
using Cat.Application;
using Microsoft.AspNetCore.Mvc;

namespace Cat.WebUI.Controllers
{
    // todo: try to implement endpoints via IBotApiEndpoint interface with dynamic registration
    public class FakeBotApiEndpointController : ApiController
    {
        [Route("update")]
        [HttpPost]
        public async Task Update(FakeBotUpdate update)
        {
            await Mediator.Send(new FakeBotUpdateCommand(update));
        }
    }
}
