using System;
using System.Linq;
using Cat.Business.Schedule.Tasks;
using Cat.Domain.Repositories;
using log4net;

namespace Telegram.Schedule.Tasks
{
    public class ProcessSubscriptionsTask : IScheduledTask
    {
        private readonly ISystemValuesRespository _systemValuesRepo;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProcessSubscriptionsTask(ISystemValuesRespository systemValuesRepo)
        {
            _systemValuesRepo = systemValuesRepo;
        }

        public void Execute()
        {

            var chatIds = _systemValuesRepo.GetAll()
                .Where(x => x.DataDescriptor.Contains("SubscriptionChatId_"))
                .ToList();

            foreach (var cId in chatIds)
            {     
                try
                {
                    _log.DebugFormat("Found chat id: '{0}'", cId.Data);
                    var chatId = Convert.ToInt64(cId.Data);
                    TelegramBot.Client.SendTextMessageAsync(chatId, "meow!");
                }
                catch (Exception e)
                {
                  _log.ErrorFormat("Error processing chat id '{0}' subscription: {1}", cId.Id, e);  
                } 
            }

        }
    }
}
