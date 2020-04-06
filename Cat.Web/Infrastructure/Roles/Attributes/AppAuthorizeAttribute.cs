using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Cat.Web.Infrastructure.Platform.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Cat.Web.Infrastructure.Roles.Attributes
{
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly List<string> _requiredRoles = new List<string>();

        public AppAuthorizeAttribute(params AppRole[] requiredRoles)
        {
            foreach (var role in requiredRoles)
            {
                _requiredRoles.Add(AppRolesHelper.RoleSystemName(role));
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var baseResult = base.IsAuthorized(actionContext);
            if (!baseResult) return false;

            var userManager = actionContext.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var curentUser = CurrentUserProvider.CurrentUser(actionContext.Request);
            if (userManager == null || curentUser == null) return false;

            return _requiredRoles.All(role => userManager.IsInRole(curentUser.Id, role));
        }
    }
}