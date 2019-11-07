using System;
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

        [HttpGet]
        public async Task<SystemLogEntriesPackage> GetNextEntries(string descriptor, string lastEntryId, int count)
        {
            var loggingService = SystemLoggingServiceFactory.CreateService(descriptor, _container);
            return await loggingService.GetNextEntriesAsync(lastEntryId, count);
        }

        [HttpPost]
        public async Task<SystemLogEntriesPackage> Clean(string descriptor, int secondsThreshold, int prevLoadedCount)
        {
            var loggingService = SystemLoggingServiceFactory.CreateService(descriptor, _container);
            await loggingService.CleanAsync(TimeSpan.FromSeconds(secondsThreshold));
            var result = await loggingService.GetNextEntriesAsync(null, prevLoadedCount);
            return result;
        }

    }
}
