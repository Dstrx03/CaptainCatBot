using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Cat.Web.Models.Users;

namespace Cat.Web.Controllers.Api
{
    [Authorize]
    public class UsersController : ApiController
    {

        private static List<AppUserViewModel> _usersFake = new List<AppUserViewModel>
        {
            new AppUserViewModel{Id = "1", Name = "admin", Roles = "admin"},
            new AppUserViewModel{Id = "2", Name = "user1", Roles = ""},
        };

        public UsersController()
        {
        }

        [HttpGet]
        public List<AppUserViewModel> GetUsersList()
        {
            return _usersFake;
        }

        [HttpDelete]
        public object RemoveUser(string id)
        {
            if (_usersFake.Count <= 1) return new { Success = false, Error = "Cannot remove the last user." };
            _usersFake.Remove(_usersFake.SingleOrDefault(x => x.Id == id));
            return new { Success = true };
        }
    }
}
