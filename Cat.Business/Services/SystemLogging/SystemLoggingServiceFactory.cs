using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StructureMap;

namespace Cat.Business.Services.SystemLogging
{
    public class SystemLoggingServiceFactory
    {
        public static ISystemLoggingServiceBase CreateService(string descriptor, IContainer container)
        {
            switch (descriptor)
            {
                case "RefresherService":
                    return container.GetInstance<RefresherLoggingService>();
                case "ATriggerService":
                    return container.GetInstance<ATriggerLoggingService>();
                case "":
                case null:
                default:
                    throw new ArgumentOutOfRangeException("descriptor", descriptor, null);
            }
        }

        public static List<ISystemLoggingServiceBase> CreateAllServices(IContainer container)
        {
            var parentType = typeof(SystemLoggingServiceBase);
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var subclasses = types.Where(t => t.IsSubclassOf(parentType)).ToList();
            return subclasses.Select(sc => container.GetInstance(sc) as ISystemLoggingServiceBase).ToList();
        }
    }
}
