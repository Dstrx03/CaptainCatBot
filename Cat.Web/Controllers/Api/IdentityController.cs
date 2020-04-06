using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Cat.Web.Infrastructure.Platform.Identity;
using Cat.Web.Infrastructure.Roles;
using Cat.Web.Models;
using Cat.Web.Models.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Cat.Web.Controllers.Api
{
    [Authorize]
    public class IdentityController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public IdentityController()
        {
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public AuthInfo GetAuthInfo()
        {
            var currentUser = CurrentUserProvider.CurrentUser(Request);

            var authUserInfo = new AuthUserInfo();
            if (currentUser != null)
            {
                var roles = AppRolesHelper.SelectUserRoles(currentUser.Id, Request);
                var rolesView = AppRolesHelper.SelectUserRolesView(roles);
                authUserInfo = new AuthUserInfo
                {
                    Name = currentUser.UserName,
                    Roles = AppRolesHelper.SelectUserRoles(currentUser.Id, Request),
                    RolesView = String.Join(", ", rolesView)
                };
            }

            return new AuthInfo
            {
                IsAuthenticated = Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated,
                AuthUserInfo = authUserInfo,
                RegisteredRoles = AppRolesRegistry.RegisteredRolesParsed
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<LoginResult> Login([FromBody] LoginViewModel model)
        {
            var signInStatus = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            var errors = new List<string>();
            switch (signInStatus)
            {
                case SignInStatus.Success:
                    break;
                case SignInStatus.LockedOut:
                    errors.Add("User is locked out.");
                    break;
                case SignInStatus.RequiresVerification:
                    errors.Add("Sign in requires additional verification.");
                    break;
                case SignInStatus.Failure:
                    errors.Add("Invalid User Name or Password.");
                    break;
                default:
                    break;

            }
            return new LoginResult { SignInStatus = signInStatus, Errors = errors.ToArray() }; 
        }

        [HttpPost]
        public async Task<bool> LogOff()
        {
            var currentUser = CurrentUserProvider.CurrentUser(Request);
            await UserManager.UpdateSecurityStampAsync(currentUser.Id);
            AuthenticationManager.SignOut();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
