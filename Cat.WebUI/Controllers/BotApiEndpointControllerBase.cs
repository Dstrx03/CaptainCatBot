using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cat.WebUI.Controllers
{
    // todo: routing, concrete area for Bot API Endpoint Controllers
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BotApiEndpointControllerBase<TUpdate> : MediatorController
    {
        public const string PathTemplateUpdate = "/api/[controller]/Update";

        [Route("update")]
        [HttpPost]
        public abstract Task Update(TUpdate update);
    }
}
