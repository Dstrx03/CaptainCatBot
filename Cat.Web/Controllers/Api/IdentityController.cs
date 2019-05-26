using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
            var context = Request.GetOwinContext();
            return new AuthInfo
            {
                IsAuthenticated = context.Authentication.User.Identity.IsAuthenticated, 
                AuthUserInfo = new AuthUserInfo { Name = context.Authentication.User.Identity.Name }
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<LoginResult> Login([FromBody] LoginViewModel model)
        {
            var signInStatus = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
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
                    errors.Add("Invalid email or password.");
                    break;
                default:
                    break;

            }
            return new LoginResult { SignInStatus = signInStatus, Errors = errors.ToArray() }; 
        }

        [HttpPost]
        public bool LogOff()
        {
            AuthenticationManager.SignOut();
            return true;
        }
    }
}
