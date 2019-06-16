
using System;
using System.Collections.Generic;
using System.Linq;
using Cat.Domain.Entities.Identity;

namespace Cat.Web.Models.Users
{
    public class AppUserViewModel
    {
        public AppUserViewModel()
        {
            Roles = new List<string>();
        }

        public AppUserViewModel(ApplicationUser user, List<string> roles, List<string> rolesView) : this()
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Roles = roles;
            RolesView = String.Join(", ", rolesView);
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string OldPassword { get; set; }

        public List<string> Roles { get; set; }

        public string RolesView { get; set; }
    }
}