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
        public RefresherSettings RefresherGetSettings()
        {
            return _refresherService.GetSettings();
        }

        [HttpPost]
        public RefresherSettings RefresherSaveSettings([FromBody] RefresherSettings settings)
        {
            var settingsResult = _refresherService.SaveSettings(settings);
            RefresherConfig.Register();
            return settingsResult;
        }

        [HttpGet]
        public ATriggerSettings ATriggerGetSettings()
        {
            return _atriggerService.GetSettings();
        }

        [HttpPost]
        public ATriggerSettings ATriggerSaveSettings([FromBody] ATriggerSettings settings)
        {
            var settingsResult = _atriggerService.SaveSettings(settings);
            return settingsResult;
        }
    }
}
