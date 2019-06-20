using System.Web.Mvc;
using Cat.Web.Infrastructure.Platform;
using Cat.Web.Infrastructure.Platform.WebApi.Attributes;

namespace Cat.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            if (AppSettings.Instance.UseHttps) filters.Add(new RequireHttpsAttribute());
        }
    }
}
