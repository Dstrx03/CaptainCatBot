using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Cat.Domain.Entities.Identity;
using Cat.Web.Infrastructure.Platform;
using Cat.Web.Infrastructure.Platform.WebApi;
using Cat.Web.Models.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Cat.Web.Controllers.Api
{
    [Authorize]
    public class UsersController : ApiController
    {
        private ApplicationUserManager _userManager;

        public UsersController()
        {
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
        public async Task<List<AppUserViewModel>> GetUsersList()
        {
            return (from user in await UserManager.Users.ToListAsync() select new AppUserViewModel(user)).ToList();
        }

        [HttpGet]
        public async Task<AppUserViewModel> GetUser(string id)
        {
            return new AppUserViewModel(await UserManager.FindByIdAsync(id));
        }

        [HttpPost]
        public async Task<CatProcedureResult> CreateUser([FromBody] AppUserViewModel userModel)
        {
            var user = new ApplicationUser { UserName = userModel.UserName, Email = userModel.Email };
            var result = await UserManager.CreateAsync(user, userModel.Password);
            return !result.Succeeded ? CatProcedureResult.Error(result.Errors.ToArray()) : CatProcedureResult.Success();
        }

        [HttpPut]
        public async Task<CatProcedureResult> UpdateUser([FromBody] AppUserViewModel userModel)
        {
            var currentUser = await CurrentUserProvider.CurrentUserAsync(Request);
            var updateAuthInfo = currentUser.Id == userModel.Id;

            var user = await UserManager.FindByIdAsync(userModel.Id);

            user.UserName = userModel.UserName;
            user.Email = userModel.Email;
            
            var updateUserRes = await UserManager.UpdateAsync(user);
            if (!updateUserRes.Succeeded) return CatProcedureResult.Error(updateUserRes.Errors.ToArray());

            if (String.IsNullOrEmpty(userModel.Password)) return CatProcedureResult.Success(updateAuthInfo);
            var changePasswordRes = await UserManager.ChangePasswordAsync(userModel.Id, userModel.OldPassword, userModel.Password);
            return !changePasswordRes.Succeeded ? CatProcedureResult.Error(changePasswordRes.Errors.ToArray(), updateAuthInfo) : CatProcedureResult.Success(updateAuthInfo);
        }

        [HttpDelete]
        public async Task<CatProcedureResult> RemoveUser(string id)
        {
            var usersLeft = await UserManager.Users.ToListAsync();
            if (usersLeft.Count <= 1) return CatProcedureResult.Error(new []{"Cannot remove the last user!"});
            var result = await UserManager.DeleteAsync(await UserManager.FindByIdAsync(id));
            return !result.Succeeded ? CatProcedureResult.Error(result.Errors.ToArray()) : CatProcedureResult.Success();
        }
    }
}
