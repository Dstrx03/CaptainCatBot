
using System.Threading.Tasks;

namespace Cat.Business.Schedule.Tasks
{
    public interface IScheduledAsyncTask
    {
        Task ExecuteAsync();
    }
}