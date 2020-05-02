using System;
using System.Linq;

namespace Cat.Business.Services.SystemLogging.Factory
{
    public enum ServiceType
    {
        [SystemLoggingServiceTypeAttribute(SystemLoggingServiceFactory.RefresherServiceDescriptor)]
        Refresher,

        [SystemLoggingServiceTypeAttribute(SystemLoggingServiceFactory.ATriggerServiceDescriptor)]
        ATrigger,

        [SystemLoggingServiceTypeAttribute(SystemLoggingServiceFactory.TelegramBotDescriptor)]
        TelegramBot
    }
}
