using System;
using System.Linq;
using System.Text;
using System.Web.Http;
using Cat.Business.Services.SystemLogging;
using Cat.Common.Helpers;
using Cat.Web.Infrastructure.Platform;
using log4net;
using StructureMap;

namespace Cat.Web.Controllers.Api
{
    public class ReverberationController : ApiController
    {
        private readonly IContainer _container;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReverberationController(IContainer container)
        {
            _container = container;
        }

        [HttpGet]
        public string Refresher()
        {
            return MaskDataHelper.MoshText(string.Format("{0} application | Powered by CaptainCatBot | Pre-alpha", AppSettings.Instance.AppTitle));
        }

        [HttpGet]
        public void ATrigger(string message)
        {
            var reverbMsg = string.Format("Received request on ATrigger Service endpoint, message: '{0}'", message);

            var loggingService = SystemLoggingServiceFactory.CreateService("ATriggerService", _container);
            loggingService.AddEntry(reverbMsg);
            _log.Debug(reverbMsg);
        }

    }
}
