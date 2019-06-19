using System;
using System.Linq;
using Cat.Business.Services.SystemLogging;
using log4net;
using StructureMap;

namespace Cat.Business.Schedule.Tasks
{
    public class CleanUpSystemLog : IScheduledTask
    {
        private readonly IContainer _container;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CleanUpSystemLog(IContainer container)
        {
            _container = container;
        }

        public void Execute()
        {
            _log.Debug("Clean up system log task started");

            var loggingServices = SystemLoggingServiceFactory.CreateAllServices(_container);

            _log.DebugFormat("Logging services found: {0}", loggingServices.Count == 0 ? "none" : string.Join(", ", loggingServices.Select(x => x.Descriptor())));

            foreach (var svc in loggingServices)
            {
                try
                {
                    _log.DebugFormat("Cleaning {0} entries older than {1} seconds ({2:F2} hours)", svc.Descriptor(), svc.DefaultSecondsThreshold(), svc.DefaultSecondsThreshold() / (60m * 60m));
                    svc.Clean(svc.DefaultSecondsThreshold());
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error while cleaning log entries for {0}: {1}", svc.Descriptor(), ex);
                }
                _log.DebugFormat("{0}'s entries older than {1} seconds ({2:F2} hours) successfully deleted", svc.Descriptor(), svc.DefaultSecondsThreshold(), svc.DefaultSecondsThreshold() / (60m * 60m));
            }

            _log.Debug("Clean up system log task completed");
        }
    }
}
