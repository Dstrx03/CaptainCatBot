using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Cat.Domain.Entities.Identity;
using Cat.Web;
using Cat.Web.Infrastructure.Platform.Identity;
using Cat.Web.Infrastructure.Platform.WebApi;
using Cat.Web.Infrastructure.Roles;
using Cat.Web.Infrastructure.Roles.Attributes;
using Cat.Web.Models.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Cat.Web.Controllers.Api
{

    [AppAuthorize(AppRole.Admin)]
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

        [HttpGet]
        public async Task<List<AppUserViewModel>> GetUsersList()
        {
            var usersList = new List<AppUserViewModel>();
            foreach(var user in await UserManager.Users.ToListAsync())
            {
                var roles = await AppRolesHelper.SelectUserRolesAsync(user.Id, Request);
                var rolesView = AppRolesHelper.SelectUserRolesView(roles);
                usersList.Add(new AppUserViewModel(user, roles, rolesView));
            }
            return usersList.OrderBy(x => x.UserName).ToList();
        }

        [HttpGet]
        public async Task<AppUserViewModel> GetUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var roles = await AppRolesHelper.SelectUserRolesAsync(id, Request);
            var rolesView = AppRolesHelper.SelectUserRolesView(roles);
            return new AppUserViewModel(user, roles, rolesView);
        }

        [HttpPost]
        public async Task<CatProcedureResult> CreateUser([FromBody] AppUserViewModel userModel)
        {
            var user = new ApplicationUser { UserName = userModel.UserName, Email = userModel.Email };
            var createRes = await UserManager.CreateAsync(user, userModel.Password);
            if (!createRes.Succeeded) return CatProcedureResult.Error(createRes.Errors.ToArray());

            var createdUser = await UserManager.FindByNameAsync(userModel.UserName);
            userModel.Id = createdUser.Id;
            var updateUserRes = await UpdateRolesAsync(userModel);
            return !updateUserRes.Succeeded ? CatProcedureResult.Error(updateUserRes.Errors.ToArray()) : CatProcedureResult.Success();
        }

        [HttpPut]
        public async Task<CatProcedureResult> UpdateUser([FromBody] AppUserViewModel userModel)
        {
            var currentUser = await CurrentUserProvider.CurrentUserAsync(Request);
            var updateAuthInfo = currentUser.Id == userModel.Id;

            var user = await UserManager.FindByIdAsync(userModel.Id);

            user.UserName = userModel.UserName;
            user.Email = userModel.Email;
            
            // Details update
            var updateUserRes = await UserManager.UpdateAsync(user);
            if (!updateUserRes.Succeeded) return CatProcedureResult.Error(updateUserRes.Errors.ToArray(), updateAuthInfo);

            // Roles update
            var updateRolesRes = await UpdateRolesAsync(userModel);
            if (!updateRolesRes.Succeeded) return CatProcedureResult.Error(updateRolesRes.Errors.ToArray(), updateAuthInfo);

            // Password update
            if (String.IsNullOrEmpty(userModel.Password)) return CatProcedureResult.Success(updateAuthInfo);
            var changePasswordRes = await UserManager.ChangePasswordAsync(userModel.Id, userModel.OldPassword, userModel.Password);
            return !changePasswordRes.Succeeded ? CatProcedureResult.Error(changePasswordRes.Errors.ToArray(), updateAuthInfo) : CatProcedureResult.Success(updateAuthInfo);
        }

        [HttpDelete]
        public async Task<CatProcedureResult> RemoveUser(string id)
        {
            var currentUser = await CurrentUserProvider.CurrentUserAsync(Request);
            if(currentUser.Id == id)
                return CatProcedureResult.Error(new[] {"Cannot remove user that is currently logged on this session!"});
            if (await IsLastAdmin(id)) 
                return CatProcedureResult.Error(new[] {String.Format("Cannot remove the last user with role {0}!", AppRolesHelper.RoleViewName(AppRole.Admin))});

            var result = await UserManager.DeleteAsync(await UserManager.FindByIdAsync(id));
            return !result.Succeeded ? CatProcedureResult.Error(result.Errors.ToArray()) : CatProcedureResult.Success();
        }



        private async Task<IdentityResult> UpdateRolesAsync(AppUserViewModel userModel)
        {
            var currentRoles = await AppRolesHelper.SelectUserRolesAsync(userModel.Id, Request);
            var addedList = new List<IdentityResult>();
            foreach (var role in userModel.Roles)
            {
                if (currentRoles.Contains(role)) continue;
                addedList.Add(await UserManager.AddToRoleAsync(userModel.Id, role));
                currentRoles.Add(role);
            }

            var removedList = new List<IdentityResult>();
            foreach (var role in currentRoles)
            {
                if (userModel.Roles.Contains(role)) continue;
                else if (await IsLastAdmin(userModel.Id) && role == AppRolesHelper.RoleSystemName(AppRole.Admin))
                {
                    removedList.Add(IdentityResult.Failed(String.Format("Cannot remove {0} role from the last admin user!", AppRolesHelper.RoleViewName(AppRole.Admin))));
                    continue;
                }
                removedList.Add(await UserManager.RemoveFromRoleAsync(userModel.Id, role));
            }

            var success = addedList.Select(x => x.Succeeded).All(x => x == true) && removedList.Select(x => x.Succeeded).All(x => x == true);
            var errors = addedList.SelectMany(x => x.Errors).ToArray().Union(removedList.SelectMany(x => x.Errors)).ToArray();

            return success ? IdentityResult.Success : IdentityResult.Failed(errors);
        }

        private async Task<bool> IsLastAdmin(string id)
        {
            var admins = new List<ApplicationUser>();
            foreach (var user in await UserManager.Users.ToListAsync())
            {
                if (await UserManager.IsInRoleAsync(user.Id, AppRolesHelper.RoleSystemName(AppRole.Admin))) 
                    admins.Add(user);
            }
            return admins.Count == 1 && admins.Single().Id == id;
        }
    }
}
