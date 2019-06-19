using System;
using System.Web.Http;
using Cat.Business.Services;
using Cat.Business.Services.InternalServices;
using Cat.Domain.Entities.SystemValues;
using Cat.Web.App_Start;
using Cat.Web.Infrastructure.Roles;
using Cat.Web.Infrastructure.Roles.Attributes;

namespace Cat.Web.Controllers.Api
{
    [AppAuthorize(AppRole.Admin)]
    public class InternalServicesController : ApiController
    {
        private readonly IRefresherManager _refresherManager;
        private readonly IATriggerService _atriggerService;

        public InternalServicesController(IRefresherManager refresherManager, IATriggerService atriggerService)
        {
            _refresherManager = refresherManager;
            _atriggerService = atriggerService;
        }

        [HttpGet]
        public RefresherSettings RefresherGetSettings()
        {
            return _refresherManager.GetSettings();
        }

        [HttpPost]
        public RefresherSettings RefresherSaveSettings([FromBody] RefresherSettings settings)
        {
            var settingsResult = _refresherManager.SaveSettings(settings);
            RefresherConfig.RegisterRefresher();
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
