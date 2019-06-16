using System;
using System.Collections.Generic;
using System.Linq;
using Cat.Domain;
using Cat.Web.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Cat.Web.Infrastructure.Roles
{
    public class AppRolesRegistry
    {
        private static readonly List<IdentityRole> _identityRoles = new List<IdentityRole>();
        private static readonly List<AppRole> _appRoles = new List<AppRole>();

        public static void Register()
        {
            AddRole(AppRole.Admin);
            AddRole(AppRole.Operator);

            SeedRoles();
        }

        public static List<AppRole> RegisteredRoles { get { return _appRoles; } }

        public static List<ParsedAppRole> RegisteredRolesParsed { get { return _appRoles.Select(x => new ParsedAppRole {ViewName = AppRolesHelper.RoleViewName(x), SystemName = AppRolesHelper.RoleSystemName(x)}).ToList(); } }

        private static void AddRole(AppRole role)
        {
            var roleName = AppRolesHelper.RoleSystemName(role);
            if (String.IsNullOrEmpty(roleName)) return;
            _identityRoles.Add(new IdentityRole(roleName));
            _appRoles.Add(role);
        }

        private static void SeedRoles()
        {
            using (var dbContext = new AppDbContext())
            {
                using (var rolesManager = new ApplicationRolesManager(new RoleStore<IdentityRole>(dbContext)))
                {
                    foreach (var ir in _identityRoles)
                    {
                        if (!rolesManager.RoleExists(ir.Name)) 
                            rolesManager.Create(ir);
                    }
                    foreach (var ir in rolesManager.Roles.ToList())
                    {
                        if (!_identityRoles.Select(x => x.Name).Contains(ir.Name)) 
                            rolesManager.Delete(ir);
                    }
                }
            }
        }

    }
}