using System.Threading.Tasks;
using System.Web.Http;
using Cat.Business.Services.InternalServices.Settings;
using Cat.Business.Services.InternalServices.Settings.SettingsModel;
using Cat.Web.App_Start;
using Cat.Web.Infrastructure.Roles;
using Cat.Web.Infrastructure.Roles.Attributes;

namespace Cat.Web.Controllers.Api
{
    [AppAuthorize(AppRole.Admin)]
    public class InternalServicesController : ApiController
    {
        private readonly RefresherSettingsManager _refresherSettingsManager;
        private readonly ATriggerSettingsManager _atriggerSettingsManager;

        public InternalServicesController(RefresherSettingsManager refresherSettingsManager, ATriggerSettingsManager atriggerSettingsManager)
        {
            _refresherSettingsManager = refresherSettingsManager;
            _atriggerSettingsManager = atriggerSettingsManager;
        }

        [HttpGet]
        public async Task<RefresherSettings> RefresherGetSettings()
        {
            return await _refresherSettingsManager.GetSettingsAsync();
        }

        [HttpPost]
        public async Task<RefresherSettings> RefresherSaveSettings([FromBody] RefresherSettings settings)
        {
            var settingsResult = await _refresherSettingsManager.SaveSettingsAsync(settings);
            RefresherConfig.Register();
            return settingsResult;
        }

        [HttpGet]
        public async Task<ATriggerSettings> ATriggerGetSettings()
        {
            return await _atriggerSettingsManager.GetSettingsAsync();
        }

        [HttpPost]
        public async Task<ATriggerSettings> ATriggerSaveSettings([FromBody] ATriggerSettings settings)
        {
            return await _atriggerSettingsManager.SaveSettingsAsync(settings);
        }
    }
}
