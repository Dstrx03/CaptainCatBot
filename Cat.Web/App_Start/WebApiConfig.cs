using System.Web.Http;
using Cat.Common.AppSettings;
using Cat.Web.Infrastructure.Platform.WebApi.Attributes;

namespace Cat.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (BaseUrlProvider.UseHttps)
            {
                var allowedPaths = new[]
                {
                    "api/v1/Reverberation/Refresher",
                    "api/v1/Reverberation/ATrigger"
                };
                config.Filters.Add(new ApiRequireHttpsAttribute(allowedPaths));
            }

            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { controller = @"^(?:(?!TelegramWebhook).)*$" } // exclude TelegramWebhookController hack
            );

            config.Routes.MapHttpRoute(
                name: "TelegramWebhook",
                routeTemplate: TelegramBotTokenProvider.ApiRouteTemplate,
                defaults: new { controller = "TelegramWebhook", action = "Post" }
            );
        }
    }
}
