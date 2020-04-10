using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Business.Schedule.Tasks;
using Cat.Domain.Repositories;
using log4net;

namespace Telegram.Schedule.Tasks
{
    public class ProcessSubscriptionsTask : IScheduledAsyncTask
    {
        private readonly ISystemValuesRespository _systemValuesRepo;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProcessSubscriptionsTask(ISystemValuesRespository systemValuesRepo)
        {
            _systemValuesRepo = systemValuesRepo;
        }

        public async Task ExecuteAsync()
        {
            var chatIds = await _systemValuesRepo.GetAll()
                .Where(x => x.DataDescriptor.Contains("SubscriptionChatId_"))
                .ToListAsync();

            foreach (var cId in chatIds)
            {     
                try
                {
                    _log.DebugFormat("Found chat id: '{0}'", cId.Data);
                    var chatId = Convert.ToInt64(cId.Data);
                    await TelegramBot.Client.SendTextMessageAsync(chatId, "meow!");
                }
                catch (Exception e)
                {
                  _log.ErrorFormat("Error processing chat id '{0}' subscription: {1}", cId.Id, e);  
                } 
            }

        }
    }
}
