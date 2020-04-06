using System.Collections.Generic;
using System.Linq;
using Cat.Web.Infrastructure.Platform.Identity;
using Cat.Web.Infrastructure.Roles;
using Hangfire.Dashboard;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Cat.Web.Infrastructure.Schedule.Hangfire
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var owinContext = new OwinContext(context.GetOwinEnvironment());

            var baseResult = owinContext.Authentication.User.Identity.IsAuthenticated;
            if (!baseResult) return false;

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var curentUser = CurrentUserProvider.CurrentUser(owinContext);
            if (userManager == null || curentUser == null) return false;

            var requredRoles = new List<string> { AppRolesHelper.RoleSystemName(AppRole.Admin) };

            return requredRoles.All(role => userManager.IsInRole(curentUser.Id, role));
        }
    }
}