using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Cat.Web.Infrastructure.Platform.Identity;
using log4net;
using Microsoft.AspNet.Identity;

namespace Cat.Web.Infrastructure.Roles
{
    public class AppRolesHelper : IdentityUtilsProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string RoleSystemName(AppRole role)
        {
            var attr = GetAttributeInfo(role);
            return attr != null ? attr.SystemName : null;
        }

        public static string RoleViewName(AppRole role)
        {
            var attr = GetAttributeInfo(role);
            return attr != null ? attr.ViewName : null;
        }

        private static AppRoleAttribute GetAttributeInfo(AppRole role)
        {
            var type = typeof(AppRole);
            var memberInfo = type.GetMember(role.ToString());
            var member = memberInfo.FirstOrDefault(m => m.DeclaringType == type);
            if (member == null)
            {
                _log.ErrorFormat("Cannot determine AppRoleAttribute for role '{0}'!", role.ToString());
                return null;
            }
            var attributes = member.GetCustomAttributes(typeof(AppRoleAttribute), false);
            return (AppRoleAttribute) attributes[0];
        }

        public static List<string> SelectUserRoles(string id, HttpRequestMessage request = null)
        {
            var userManager = GetUserManager(GetCurrentContext(request));
            return userManager.GetRoles(id).ToList();
        }

        public static async Task<List<string>> SelectUserRolesAsync(string id, HttpRequestMessage request = null)
        {
            var userManager = GetUserManager(GetCurrentContext(request));
            var roles = await userManager.GetRolesAsync(id);
            return roles.ToList();
        }

        public static List<string> SelectUserRolesView(List<string> roles)
        {
            return (from r in AppRolesRegistry.RegisteredRolesParsed where roles.Contains(r.SystemName) select r.ViewName).ToList();
        }

    }
}