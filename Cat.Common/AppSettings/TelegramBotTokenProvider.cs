using System.Web.Configuration;

namespace Cat.Common.AppSettings
{
    public class TelegramBotTokenProvider
    {
        private static readonly string _telegramBotToken;

        static TelegramBotTokenProvider()
        {
            _telegramBotToken = GetTelegramBotToken();
        }

        public static string Token { get { return _telegramBotToken; } }

        public static string ApiRouteTemplate { get { return _telegramBotToken != null ? string.Format("{0}", _telegramBotToken.Replace(":", string.Empty)) : null; } }

        private static string GetTelegramBotToken()
        {
            var telegramBotToken = WebConfigurationManager.AppSettings["TelegramBotToken"];
            return string.IsNullOrEmpty(telegramBotToken) ? null : telegramBotToken;
        }
    }
}