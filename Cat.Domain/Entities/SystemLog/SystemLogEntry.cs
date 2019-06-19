using System;

namespace Cat.Domain.Entities.SystemLog
{
    public class SystemLogEntry
    {
        public SystemLogEntry()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string EntryDescriptor { get; set; }

        public DateTime EntryDate { get; set; }

        public string Entry { get; set; }
    }
}
