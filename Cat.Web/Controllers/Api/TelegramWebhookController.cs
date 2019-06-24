using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Telegram;
using Telegram.Bot.Types;

namespace Cat.Web.Controllers.Api
{
    public class TelegramWebhookController : ApiController
    {
        private readonly ITelegramService _telegramService;
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TelegramWebhookController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Update update)
        {
            await _telegramService.ProcessUpdate(update);
            return Ok();
        }

    }
}
