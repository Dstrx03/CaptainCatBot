using System.Data.Entity;
using Cat.Business.Services.SystemLogging;
using Cat.Common.AppSettings;
using Cat.Domain;
using Cat.Web.App_Start;
using Cat.Web.Infrastructure.Platform;
using Telegram;

namespace Cat.Web
{
    public static class TelegramBotConfig
    {
        public static void Register()
        {
            using (var dbContext = new AppDbContext())
            {
                using (var container = StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer())
                {
                    container.Configure(x =>
                    {
                        x.For<AppDbContext>().Use(dbContext);
                        x.For<DbContext>().Use(dbContext);
                    });

                    var loggingService = SystemLoggingServiceFactory.CreateService("TelegramBot", container);

                    TelegramBot.RegisterClient(TelegramBotTokenProvider.Token, loggingService);
                    TelegramBot.RegisterWebhook(AppSettings.InstanceTelegram.WebhookUrl, AppSettings.InstanceTelegram.NeedPublicCert, loggingService);
                }
            }
        }

        public static void Unregister()
        {
            using (var dbContext = new AppDbContext())
            {
                using (var container = StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer())
                {
                    container.Configure(x =>
                    {
                        x.For<AppDbContext>().Use(dbContext);
                        x.For<DbContext>().Use(dbContext);
                    });

                    var loggingService = SystemLoggingServiceFactory.CreateService("TelegramBot", container);

                    TelegramBot.UnregisterWebhook(loggingService);
                    TelegramBot.UnregisterClient(loggingService);
                }
            }
        }
    }
}
