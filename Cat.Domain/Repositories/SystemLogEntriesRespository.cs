using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Entities.SystemLog;
using Cat.Domain.Repositories.Base;

namespace Cat.Domain.Repositories
{
    public interface ISystemLogEntriesRespository : IRepository<SystemLogEntry, string>
    {
        IQueryable<SystemLogEntry> GetEntries(string descriptor);

        IQueryable<SystemLogEntry> GetNextEntries(string descriptor, string lastEntryId, int count);

        Task<IQueryable<SystemLogEntry>> GetNextEntriesAsync(string descriptor, string lastEntryId, int count);
    }

    public class SystemLogEntriesRespository : Repository<SystemLogEntry, string>, ISystemLogEntriesRespository
    {
        public SystemLogEntriesRespository(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<SystemLogEntry> GetEntries(string descriptor)
        {
            return GetAll()
                .Where(x => x.EntryDescriptor == descriptor)
                .OrderByDescending(x => x.EntryDate);
        }

        public IQueryable<SystemLogEntry> GetNextEntries(string descriptor, string lastEntryId, int count)
        {
            var lastEntry = GetAll().SingleOrDefault(x => x.Id == lastEntryId);
            if (lastEntry == null)
                return GetAll()
                    .Where(x => x.EntryDescriptor == descriptor)
                    .OrderByDescending(x => x.EntryDate)
                    .Take(count);

            return GetAll()
                .Where(x => x.EntryDescriptor == descriptor && x.EntryDate <= lastEntry.EntryDate && x.Id != lastEntryId)
                .OrderByDescending(x => x.EntryDate)
                .Take(count);
        }

        public async Task<IQueryable<SystemLogEntry>> GetNextEntriesAsync(string descriptor, string lastEntryId, int count)
        {
            var lastEntry = await GetAll().SingleOrDefaultAsync(x => x.Id == lastEntryId);
            if (lastEntry == null)
                return GetAll()
                    .Where(x => x.EntryDescriptor == descriptor)
                    .OrderByDescending(x => x.EntryDate)
                    .Take(count);

            return GetAll()
                .Where(x => x.EntryDescriptor == descriptor && x.EntryDate <= lastEntry.EntryDate && x.Id != lastEntryId)
                .OrderByDescending(x => x.EntryDate)
                .Take(count);
        }
    }
}
