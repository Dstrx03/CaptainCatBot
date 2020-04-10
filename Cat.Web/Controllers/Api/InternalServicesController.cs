using System.Threading.Tasks;
using System.Web.Http;
using Cat.Business.Services.InternalServices;
using Cat.Web.App_Start;
using Cat.Web.Infrastructure.Roles;
using Cat.Web.Infrastructure.Roles.Attributes;

namespace Cat.Web.Controllers.Api
{
    [AppAuthorize(AppRole.Admin)]
    public class InternalServicesController : ApiController
    {
        private readonly IRefresherService _refresherService;
        private readonly IATriggerService _atriggerService;

        public InternalServicesController(IRefresherService refresherManager, IATriggerService atriggerService)
        {
            _refresherService = refresherManager;
            _atriggerService = atriggerService;
        }

        [HttpGet]
        public async Task<RefresherSettings> RefresherGetSettings()
        {
            return await _refresherService.GetSettingsAsync();
        }

        [HttpPost]
        public async Task<RefresherSettings> RefresherSaveSettings([FromBody] RefresherSettings settings)
        {
            var settingsResult = await _refresherService.SaveSettingsAsync(settings);
            RefresherConfig.Register();
            return settingsResult;
        }

        [HttpGet]
        public async Task<ATriggerSettings> ATriggerGetSettings()
        {
            return await _atriggerService.GetSettingsAsync();
        }

        [HttpPost]
        public async Task<ATriggerSettings> ATriggerSaveSettings([FromBody] ATriggerSettings settings)
        {
            var settingsResult = await _atriggerService.SaveSettingsAsync(settings);
            return settingsResult;
        }
    }
}
