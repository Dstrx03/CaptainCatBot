using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        SystemLogEntriesPackage GetNextEntries(string lastEntryId, int count);

        Task<SystemLogEntriesPackage> GetNextEntriesAsync(string lastEntryId, int count);

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
            return _logEntriesRepo.GetEntries(_descriptor).ToList();
        }

        public async Task<List<SystemLogEntry>> GetEntriesAsync()
        {
            return await _logEntriesRepo.GetEntries(_descriptor).ToListAsync();
        }

        public SystemLogEntriesPackage GetNextEntries(string lastEntryId, int count)
        {
            var entries = _logEntriesRepo.GetNextEntries(_descriptor, lastEntryId, count).ToList();
            return new SystemLogEntriesPackage(entries, IsLastPackage(entries));
        }

        public async Task<SystemLogEntriesPackage> GetNextEntriesAsync(string lastEntryId, int count)
        {
            var query = await _logEntriesRepo.GetNextEntriesAsync(_descriptor, lastEntryId, count);
            var entries = await query.ToListAsync();
            return new SystemLogEntriesPackage(entries, IsLastPackage(entries));
        }

        private bool IsLastPackage(List<SystemLogEntry> entries)
        {
            var lastEntry = entries.LastOrDefault();
            var lastEntryTotal = _logEntriesRepo.GetAll()
                .Where(x => x.EntryDescriptor == _descriptor)
                .OrderBy(x => x.EntryDate)
                .FirstOrDefault();

            if (lastEntry == null || lastEntryTotal == null) return true;
            if (lastEntry.Id == lastEntryTotal.Id) return true;
            return false;
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

    public class SystemLogEntriesPackage
    {
        public List<SystemLogEntry> Entries { get; set; }

        public bool IsLast { get; set; }

        public SystemLogEntriesPackage(List<SystemLogEntry> entries, bool isLast)
        {
            Entries = entries;
            IsLast = isLast;
        }
    }
}
