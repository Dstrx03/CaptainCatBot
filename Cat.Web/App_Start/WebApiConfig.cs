using System.Web.Http;
using System.Web.Http.Cors;
using Cat.Common.AppSettings;
using Cat.Web.Infrastructure.Platform;
using Cat.Web.Infrastructure.Platform.WebApi.Attributes;

namespace Cat.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (BaseUrlProvider.UseHttps) config.Filters.Add(new ApiRequireHttpsAttribute());

            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { controller = @"^(?:(?!TelegramWebhook).)*$" } // exclude TelegramController hack
            );

            config.Routes.MapHttpRoute(
                name: "TelegramWebhook",
                routeTemplate: TelegramBotTokenProvider.ApiRouteTemplate,
                defaults: new { controller = "TelegramWebhook", action = "Post" }
            );
        }
    }
}
