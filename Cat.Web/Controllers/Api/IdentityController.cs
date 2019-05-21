using System;
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
            switch (signInStatus)
            {
                case SignInStatus.Success:
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                    return new LoginResult { SignInStatus = signInStatus };
                case SignInStatus.Failure:
                default:
                    return new LoginResult { SignInStatus = signInStatus, Errors = new[] { "login faulire!" } }; // TODO: errors
            }
        }

        [HttpPost]
        public bool LogOff()
        {
            AuthenticationManager.SignOut();
            return true;
        }
    }
}
