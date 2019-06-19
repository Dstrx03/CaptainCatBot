using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Entities.SystemLog;
using Cat.Domain.Repositories;

namespace Cat.Business.Services.SystemLogging
{
    public interface ISystemLoggingServiceBase
    {
        int DefaultSecondsThreshold();

        string Descriptor();

        List<SystemLogEntry> GetEntries();

        Task<List<SystemLogEntry>> GetEntriesAsync();

        SystemLogEntry AddEntry(string entry);

        Task<SystemLogEntry> AddEntryAsync(string entry);

        void Clean(int secondsThreshold, DateTime? date = null);

        Task CleanAsync(int secondsThreshold, DateTime? date = null);
    }

    public abstract class SystemLoggingServiceBase : ISystemLoggingServiceBase
    {
        protected readonly ISystemLogEntriesRespository _logEntriesRepo;
        protected readonly string _descriptor;

        protected SystemLoggingServiceBase(ISystemLogEntriesRespository logEntriesRepo, string descriptor)
        {
            _logEntriesRepo = logEntriesRepo;
            _descriptor = descriptor;
        }

        public virtual int DefaultSecondsThreshold()
        {
            // 2 weeks
            return 60*60*24*7*2;
        }

        public string Descriptor()
        {
            return _descriptor;
        }

        public List<SystemLogEntry> GetEntries()
        {
            return _logEntriesRepo.GetAll().Where(x => x.EntryDescriptor == _descriptor).OrderByDescending(x => x.EntryDate).ToList();
        }

        public async Task<List<SystemLogEntry>> GetEntriesAsync()
        {
            return await _logEntriesRepo.GetAll().Where(x => x.EntryDescriptor == _descriptor).OrderByDescending(x => x.EntryDate).ToListAsync();
        }

        public SystemLogEntry AddEntry(string entry)
        {
            var logEntry = Add(entry);
            _logEntriesRepo.SaveChanges();
            return logEntry;
        }

        public async Task<SystemLogEntry> AddEntryAsync(string entry)
        {
            var logEntry = Add(entry);
            await _logEntriesRepo.SaveChangesAsync();
            return logEntry;
        }

        private SystemLogEntry Add(string entry)
        {
            var logEntry = new SystemLogEntry
            {
                Entry = entry,
                EntryDate = DateTime.Now,
                EntryDescriptor = _descriptor
            };
            _logEntriesRepo.Add(logEntry);
            return logEntry;
        }

        public void Clean(int secondsThreshold, DateTime? date = null)
        {
            if (!date.HasValue) date = DateTime.Now;
            foreach (var e in GetEntries())
            {
                if ((date.Value - e.EntryDate).TotalSeconds > secondsThreshold)
                    _logEntriesRepo.Remove(e.Id);
            }
            _logEntriesRepo.SaveChanges();
        }

        public async Task CleanAsync(int secondsThreshold, DateTime? date = null)
        {
            if (!date.HasValue) date = DateTime.Now;
            foreach (var e in await GetEntriesAsync())
            {
                if ((date.Value - e.EntryDate).TotalSeconds > secondsThreshold) 
                    await _logEntriesRepo.RemoveAsync(e.Id);
            }
            await _logEntriesRepo.SaveChangesAsync();
        }
        
    }
}
