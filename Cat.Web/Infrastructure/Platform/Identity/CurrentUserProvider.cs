using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cat.Domain.Entities.Identity;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;

namespace Cat.Web.Infrastructure.Platform.Identity
{
    public class CurrentUserProvider : IdentityUtilsProvider
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
                _log.ErrorFormat("Cannot determine current user! Exception: " + e);
                return null;
            }
        }

        public static ApplicationUser CurrentUser(IOwinContext context)
        {
            try
            {
                return FindCurrentUser(GetUserManager(context), context);
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Cannot determine current user! Exception: " + e);
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
                _log.ErrorFormat("Cannot determine current user! Exception: " + e);
                return null;
            }
        }

        public static async Task<ApplicationUser> CurrentUserAsync(IOwinContext context)
        {
            try
            {
                return await FindCurrentUserAsync(GetUserManager(context), context);
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Cannot determine current user! Exception: " + e);
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

        
    }
}