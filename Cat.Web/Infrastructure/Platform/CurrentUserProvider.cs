using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Cat.Domain.Entities.Identity;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Cat.Web.Infrastructure.Platform
{
    public class CurrentUserProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ApplicationUser CurrentUser(HttpRequestMessage request = null)
        {
            try
            {
                var context = GetCurrentContext(request);
                return FindCurrentUser(GetUserManager(context), context);
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Cannot determine current user! Stack trace: " + e.StackTrace);
                return null;
            }
        }

        public static async Task<ApplicationUser> CurrentUserAsync(HttpRequestMessage request = null)
        {
            try
            {
                var context = GetCurrentContext(request);
                return await FindCurrentUserAsync(GetUserManager(context), context);
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Cannot determine current user! Stack trace: " + e.StackTrace);
                return null;
            }
        }

        private static ApplicationUser FindCurrentUser(ApplicationUserManager userManager, IOwinContext context)
        {
            return userManager.FindById(context.Authentication.User.Identity.GetUserId());
        }

        private static async Task<ApplicationUser> FindCurrentUserAsync(ApplicationUserManager userManager, IOwinContext context)
        {
            return await userManager.FindByIdAsync(context.Authentication.User.Identity.GetUserId());
        }

        private static ApplicationUserManager GetUserManager(IOwinContext context)
        {
            return context.GetUserManager<ApplicationUserManager>();
        }

        private static IOwinContext GetCurrentContext(HttpRequestMessage request = null)
        {
            if (request == null) 
                request = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
            return request.GetOwinContext();
        }
    }
}