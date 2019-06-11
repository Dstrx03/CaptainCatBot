
using Cat.Domain.Entities.Identity;

namespace Cat.Web.Models.Users
{
    public class AppUserViewModel
    {
        public AppUserViewModel()
        {
        }

        public AppUserViewModel(ApplicationUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string OldPassword { get; set; }
    }
}