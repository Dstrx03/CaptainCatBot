using System;
using System.Web.Configuration;
using Cat.Common.AppSettings.Providers;

namespace Cat.Common.AppSettings.Assemblies
{
    public interface ITelegramAppSettings
    {
        string WebhookUrl { get; }

        bool NeedPublicCert { get; }
    }

    public class TelegramAppSettings : ITelegramAppSettings
    {
        public string WebhookUrl
        {
            get
            {
                return string.Format("{0}{1}", BaseUrlProvider.BaseUrl, TelegramBotTokenProvider.ApiRouteTemplate);
            }
        }

        public bool NeedPublicCert
        {
            get
            {
                return Convert.ToBoolean(WebConfigurationManager.AppSettings["NeedPublicCert"]);
            }
        }
    }
}
