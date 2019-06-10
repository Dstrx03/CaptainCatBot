using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Cat.Web.Infrastructure.Platform.WebApi;
using Cat.Web.Models.Users;

namespace Cat.Web.Controllers.Api
{
    [Authorize]
    public class UsersController : ApiController
    {

        private static List<AppUserViewModel> _usersFake = new List<AppUserViewModel>
        {
            new AppUserViewModel{Id = "1", UserName = "admin", Email = "qqqqq1@mail.com"},
            new AppUserViewModel{Id = "2", UserName = "user1", Email = "qqqqq2@mail.com"},
        };

        public UsersController()
        {
        }

        [HttpGet]
        public List<AppUserViewModel> GetUsersList()
        {
            return _usersFake;
        }

        [HttpGet]
        public AppUserViewModel GetUser(string id)
        {
            return _usersFake.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public CatProcedureResult AddUser([FromBody] AppUserViewModel userModel)
        {
            //todo: rename to RegisterUser?
            if (_usersFake.Select(x => x.UserName).Contains(userModel.UserName))
                return CatProcedureResult.Error(new[] {"User Name " + userModel.UserName + " is alredy taken by another user..."});
            if (_usersFake.Select(x => x.Email).Contains(userModel.Email)) 
                return CatProcedureResult.Error(new[] {"Email " + userModel.Email + " is alredy taken by another user..."});
            userModel.Id = (_usersFake.Select(x => Convert.ToInt32(x.Id)).Max() + 1).ToString();
            userModel.Password = null;
            _usersFake.Add(userModel);
            return CatProcedureResult.Success();
        }

        [HttpPut]
        public CatProcedureResult UpdateUser([FromBody] AppUserViewModel userModel)
        {
            //TODO: messages!
            var targetUser = _usersFake.FirstOrDefault(x => x.Id == userModel.Id);
            if (_usersFake.Where(x => x.Id != userModel.Id).Select(x => x.UserName).Contains(userModel.UserName))
                return CatProcedureResult.Error(new[] {"User Name " + userModel.UserName + " is alredy taken by another user..."});
            if (_usersFake.Where(x => x.Id != userModel.Id).Select(x => x.Email).Contains(userModel.Email))
                return CatProcedureResult.Error(new[] {"Email " + userModel.Email + " is alredy taken by another user..."}); 
            _usersFake.Remove(targetUser);
            userModel.Password = null;
            _usersFake.Add(userModel);
            return CatProcedureResult.Success();
        }

        [HttpDelete]
        public CatProcedureResult RemoveUser(string id)
        {
            if (_usersFake.Count <= 1) return CatProcedureResult.Error(new[] { "Cannot remove the last user!" });
            _usersFake.Remove(_usersFake.SingleOrDefault(x => x.Id == id));
            return CatProcedureResult.Success();
        }
    }
}
