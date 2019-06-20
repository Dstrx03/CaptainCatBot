using System.Web.Http;
using System.Web.Http.Cors;
using Cat.Web.Infrastructure.Platform;
using Cat.Web.Infrastructure.Platform.WebApi.Attributes;

namespace Cat.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (AppSettings.Instance.UseHttps) config.Filters.Add(new ApiRequireHttpsAttribute());

            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
