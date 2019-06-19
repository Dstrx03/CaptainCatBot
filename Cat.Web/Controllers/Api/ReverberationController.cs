using System.Web.Http;
using Cat.Business.Services.SystemLogging;
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
        public void ATrigger(string message)
        {
            var reverbMsg = string.Format("Received request on ATrigger Service endpoint: '{0}'", message);

            var loggingService = SystemLoggingServiceFactory.CreateService("ATriggerService", _container);
            loggingService.AddEntry(reverbMsg);
            _log.Debug(reverbMsg);
        }

    }
}
