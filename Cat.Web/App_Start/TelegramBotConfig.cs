using System.Data.Entity;
using System.Net;
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

                    /*
                     * From 6 Feb 2020 (approx.) Telegram disabled security protocols older than TLS v1.2.
                     *
                     * 'Starting with the .NET Framework 4.7, the default value of this property (ServicePointManager.SecurityProtocol)
                     * is SecurityProtocolType.SystemDefault. This allows .NET Framework networking APIs based on SslStream (such as FTP, HTTP, and SMTP)
                     * to inherit the default security protocols from the operating system or from any custom configurations performed by a system administrator.'
                     * (https://docs.microsoft.com/en-us/dotnet/api/system.net.servicepointmanager.securityprotocol?view=netframework-4.8)
                     *
                     * Due this we should set security protocol to TLS v1.2 explicitly to ensure that we use supported security protocol version.
                     * TODO: Think about workaround and avoid this kind of constraint.
                     */
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

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
