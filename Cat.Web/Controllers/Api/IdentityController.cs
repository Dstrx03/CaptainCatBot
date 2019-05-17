using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Cat.Web.Models;
using Microsoft.AspNet.Identity;
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

        // TODO: refactor all methods design!

        [HttpGet]
        [AllowAnonymous]
        public bool IsAuthenticated()
        {
            return Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<String> Login([FromBody] LoginViewModel model)
        {
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return "Login Success! Username: '" + Request.GetOwinContext().Authentication.User.Identity.GetUserName() + "' IsAuthenticated=" + Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
                case SignInStatus.LockedOut:
                    return "Login LockedOut!";
                case SignInStatus.RequiresVerification:
                    return "Login RequiresVerification!";
                case SignInStatus.Failure:
                default:
                    return "Login Failure!";
            }
        }

        [HttpPost]
        public string LogOff()
        {
            var name = Request.GetOwinContext().Authentication.User.Identity.GetUserName();
            var id = Request.GetOwinContext().Authentication.User.Identity.GetUserId();
            var isLogged = Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
            AuthenticationManager.SignOut();
            return string.Format("'{0}'({1}-{2}) logged off", name, id, isLogged);
        }
    }
}
