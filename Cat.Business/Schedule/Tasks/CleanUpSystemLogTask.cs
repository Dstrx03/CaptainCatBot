using System;
using System.Collections.Generic;
using System.Linq;
using Cat.Business.Services.SystemLogging;
using log4net;
using StructureMap;

namespace Cat.Business.Schedule.Tasks
{
    public class CleanUpSystemLogTask : IScheduledTask
    {
        private readonly IContainer _container;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CleanUpSystemLogTask(IContainer container)
        {
            _container = container;
        }

        public void Execute()
        {
            var loggingServices = SystemLoggingServiceFactory.CreateAllServices(_container);

            _log.DebugFormat("Logging services found: {0}", loggingServices.Count == 0 ? "none" : string.Join(", ", loggingServices.Select(x => x.Descriptor())));

            var removedIds = new List<string>();
            foreach (var svc in loggingServices)
            {
                var cleanThreshold = svc.CleanThreshold();
                try
                {
                    _log.DebugFormat("Cleaning {0} entries older than {1}", svc.Descriptor(), cleanThreshold);
                    removedIds = svc.Clean(cleanThreshold);
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error while cleaning log entries for {0}: {1}", svc.Descriptor(), ex);
                }
                _log.DebugFormat("{0}'s entries older than {1} successfully cleaned ({2} entries deleted)", svc.Descriptor(), cleanThreshold, removedIds.Count);
            }
        }
    }
}
