using System.Net.Http;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Cat.Web.Infrastructure.Platform.Identity
{
    public abstract class IdentityUtilsProvider
    {
        protected static ApplicationUserManager GetUserManager(IOwinContext context)
        {
            return context.GetUserManager<ApplicationUserManager>();
        }

        protected static IOwinContext GetCurrentContext(HttpRequestMessage request = null)
        {
            if (request == null)
                request = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
            return request.GetOwinContext();
        }
    }
}