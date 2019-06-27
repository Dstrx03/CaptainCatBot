using System;

namespace Cat.Domain.Entities.SystemValues
{
    public class SystemValue
    {
        public SystemValue()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string DataDescriptor { get; set; }

        public SystemValueType DataType { get; set; }

        public string Data { get; set; }
    }

    public enum SystemValueType
    {
        Unknown = 0,
        Boolean = 1,
        String = 2,
        Int = 3,
        DateTime = 4,
        Long = 5
    }
}
