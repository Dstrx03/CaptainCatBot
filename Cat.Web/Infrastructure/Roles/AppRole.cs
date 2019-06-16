
using System;

namespace Cat.Web.Infrastructure.Roles
{
    public enum AppRole
    {

        [AppRoleAttribute("_admin", "Admin")]
        Admin,

        [AppRoleAttribute("_operator", "Operator")]
        Operator

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AppRoleAttribute : Attribute
    {
        public AppRoleAttribute(string systemName, string viewName)
        {
            SystemName = systemName;
            ViewName = viewName;
        }

        public string SystemName { get; private set; }

        public string ViewName { get; private set; }
    }

}