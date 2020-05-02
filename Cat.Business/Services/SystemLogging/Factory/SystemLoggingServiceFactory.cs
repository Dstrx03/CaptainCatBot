using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using StructureMap;

namespace Cat.Business.Services.SystemLogging.Factory
{
    public class SystemLoggingServiceFactory
    {
        public const string RefresherServiceDescriptor = "RefresherService";
        public const string ATriggerServiceDescriptor = "ATriggerService";
        public const string TelegramBotDescriptor = "TelegramBot";

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ISystemLoggingServiceBase CreateService(ServiceType serviceType, IContainer container)
        {
            return CreateService(GetServiceDescriptor(serviceType), container);
        }

        public static ISystemLoggingServiceBase CreateService(string descriptor, IContainer container)
        {
            switch (descriptor)
            {
                case RefresherServiceDescriptor:
                    return container.GetInstance<RefresherLoggingService>();
                case ATriggerServiceDescriptor:
                    return container.GetInstance<ATriggerLoggingService>();
                case TelegramBotDescriptor:
                    return container.GetInstance<TelegramBotLoggingService>();
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

        private static string GetServiceDescriptor(ServiceType serviceType)
        {
            var type = typeof(ServiceType);
            var memberInfo = type.GetMember(serviceType.ToString());
            var member = memberInfo.FirstOrDefault(m => m.DeclaringType == type);
            if (member == null)
            {
                _log.ErrorFormat("Cannot determine SystemLoggingServiceTypeAttribute for service type '{0}'!", serviceType.ToString());
                return null;
            }
            var attributes = member.GetCustomAttributes(typeof(SystemLoggingServiceTypeAttribute), false);
            var attr = (SystemLoggingServiceTypeAttribute)attributes[0];
            return attr?.Descriptor;
        }
    }
}
