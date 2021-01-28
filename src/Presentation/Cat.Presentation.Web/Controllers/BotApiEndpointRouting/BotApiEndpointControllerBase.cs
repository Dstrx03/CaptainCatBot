using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using static Cat.Presentation.BotApiEndpointRouting.Services.BotApiEndpointRoutingPathFormatUtils;

namespace Cat.Presentation.Web.Controllers.BotApiEndpointRouting
{
    // todo: routing, concrete area for Bot API Endpoint Controllers
    [ApiController]
    [Route(BasePathTemplate)]
    public abstract class BotApiEndpointControllerBase<TUpdate, TUpdateResult> : MediatorController where TUpdateResult : Task
    {
        public const string UpdatePathTemplate = BasePathTemplate + "/Update";

        [HttpPost]
        [Route("update")]
        public abstract TUpdateResult Update(TUpdate update);
    }
}
