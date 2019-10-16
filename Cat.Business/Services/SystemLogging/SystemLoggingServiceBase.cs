using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Entities.SystemLog;
using Cat.Domain.Repositories;
using Microsoft.AspNet.SignalR;

namespace Cat.Business.Services.SystemLogging
{
    public interface ISystemLoggingServiceBase
    {
        TimeSpan CleanThreshold();

        string Descriptor();

        List<SystemLogEntry> GetEntries();

        Task<List<SystemLogEntry>> GetEntriesAsync();

        SystemLogEntry AddEntry(string entry);

        Task<SystemLogEntry> AddEntryAsync(string entry);

        List<string> Clean(TimeSpan threshold, DateTime? date = null);

        Task<List<string>> CleanAsync(TimeSpan threshold, DateTime? date = null);
    }

    public abstract class SystemLoggingServiceBase : ISystemLoggingServiceBase
    {
        protected readonly ISystemLogEntriesRespository _logEntriesRepo;
        private readonly string _descriptor;


        protected SystemLoggingServiceBase(ISystemLogEntriesRespository logEntriesRepo, string descriptor)
        {
            _logEntriesRepo = logEntriesRepo;
            _descriptor = descriptor;
        }

        public virtual TimeSpan CleanThreshold()
        {
            // 2 weeks
            return TimeSpan.FromDays(14);
        }

        protected virtual IHubContext HubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<SystemLoggingServiceHub>();
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

            HubContext().Clients.All.entryAdded(logEntry);

            return logEntry;
        }

        public async Task<SystemLogEntry> AddEntryAsync(string entry)
        {
            var logEntry = Add(entry);
            await _logEntriesRepo.SaveChangesAsync();

            HubContext().Clients.All.entryAdded(logEntry);

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

        public List<string> Clean(TimeSpan threshold, DateTime? date = null)
        {
            if (!date.HasValue) date = DateTime.Now;

            var removedIds = new List<string>();
            foreach (var e in GetEntries())
            {
                if ((date.Value - e.EntryDate).TotalSeconds > threshold.TotalSeconds)
                {
                    _logEntriesRepo.Remove(e.Id);
                    removedIds.Add(e.Id);
                }
            }

            _logEntriesRepo.SaveChanges();

            HubContext().Clients.All.entriesRemoved(removedIds);

            return removedIds;
        }

        public async Task<List<string>> CleanAsync(TimeSpan threshold, DateTime? date = null)
        {
            if (!date.HasValue) date = DateTime.Now;

            var removedIds = new List<string>();
            foreach (var e in await GetEntriesAsync())
            {
                if ((date.Value - e.EntryDate).TotalSeconds > threshold.TotalSeconds)
                {
                    await _logEntriesRepo.RemoveAsync(e.Id);
                    removedIds.Add(e.Id);
                }
            }

            await _logEntriesRepo.SaveChangesAsync();

            HubContext().Clients.All.entriesRemoved(removedIds);

            return removedIds;
        }
        
    }

    public class SystemLoggingServiceHub : Hub
    {
    }

}
