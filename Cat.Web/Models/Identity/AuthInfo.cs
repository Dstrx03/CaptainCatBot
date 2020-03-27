
using System.Collections.Generic;
using Cat.Web.Infrastructure.Roles;

namespace Cat.Web.Models.Identity
{
    public class AuthInfo
    {
        public bool IsAuthenticated { get; set; }

        public AuthUserInfo AuthUserInfo { get; set; }

        public List<ParsedAppRole> RegisteredRoles = new List<ParsedAppRole>();
    }

    public class AuthUserInfo
    {
        public string Name { get; set; }

        public List<string> Roles { get; set; }

        public string RolesView { get; set; }
    }

    public class ParsedAppRole
    {
        public string ViewName { get; set; }

        public string SystemName { get; set; }
    }
}