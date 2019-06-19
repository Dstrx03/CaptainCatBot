using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Cat.Business.Services.SystemLogging;
using Cat.Domain.Entities.SystemLog;
using Cat.Web.Infrastructure.Roles;
using Cat.Web.Infrastructure.Roles.Attributes;
using StructureMap;

namespace Cat.Web.Controllers.Api
{
    [AppAuthorize(AppRole.Admin)]
    public class SystemLoggingController : ApiController
    {
        private readonly IContainer _container;

        public SystemLoggingController(IContainer container)
        {
            _container = container;
        }

        [HttpGet]
        public async Task<List<SystemLogEntry>> GetEntries(string descriptor)
        {
            var loggingService = SystemLoggingServiceFactory.CreateService(descriptor, _container);
            return await loggingService.GetEntriesAsync();
        }

        [HttpPost]
        public async Task<List<SystemLogEntry>> Clean(string descriptor, int secondsThreshold)
        {
            var loggingService = SystemLoggingServiceFactory.CreateService(descriptor, _container);
            await loggingService.CleanAsync(secondsThreshold);
            var result = await loggingService.GetEntriesAsync();
            return result;
        }

    }
}
