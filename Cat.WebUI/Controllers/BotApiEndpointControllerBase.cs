using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cat.WebUI.Controllers
{
    // todo: routing, concrete area for Bot API Endpoint Controllers
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BotApiEndpointControllerBase<TUpdate, TUpdateResult> : MediatorController where TUpdateResult : Task
    {
        public const string PathTemplateUpdate = "/api/[controller]/Update";

        [HttpPost]
        [Route("update")]
        public abstract TUpdateResult Update(TUpdate update);
    }
}
