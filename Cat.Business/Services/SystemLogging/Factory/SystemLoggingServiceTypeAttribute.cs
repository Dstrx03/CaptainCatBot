using System;

namespace Cat.Business.Services.SystemLogging.Factory
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SystemLoggingServiceTypeAttribute : Attribute
    {
        public SystemLoggingServiceTypeAttribute(string descriptor)
        {
            Descriptor = descriptor;
        }

        public string Descriptor { get; private set; }
    }
}
