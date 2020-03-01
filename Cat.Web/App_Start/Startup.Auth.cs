using System;
using System.Collections.Generic;
using System.Linq;
using Cat.Domain;
using Cat.Domain.Entities.Identity;
using Cat.Web.Infrastructure.Platform;
using Cat.Web.Infrastructure.Platform.Identity;
using Cat.Web.Infrastructure.Roles;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Cat.Web
{
    public partial class Startup
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(() => new AppDbContext());
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRolesManager>(ApplicationRolesManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieName = "Cat.App",
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = CatSecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(1),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
                    OnApplyRedirect = ctx => {
                        // since all auth mechanics is being managed in Angular and ASP NET app used more like Web API app - no redirects to Login page from endpoint
                    }
                },
                SlidingExpiration = true
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});

            AppRolesRegistry.Register();
            SeedAdminUser();
        }

        private void SeedAdminUser()
        {
            using (var dbContext = new AppDbContext())
            {
                using (var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext)))
                {
                    var admins = new List<ApplicationUser>();
                    foreach (var user in userManager.Users.ToList())
                    {
                        if (userManager.IsInRole(user.Id, AppRolesHelper.RoleSystemName(AppRole.Admin)))
                            admins.Add(user);
                    }
                    if (admins.Count > 0) return;
                    _log.Debug("Creating obligatory admin user...");
                    var result = userManager.Create(new ApplicationUser { UserName = AppSettings.Instance.ObligatoryAdminName }, AppSettings.Instance.ObligatoryAdminPassword);
                    if (!result.Succeeded)
                    {
                        _log.ErrorFormat("Error(s) occured while creating obligatory admin user: {0}", string.Join(", ", result.Errors));
                        return;
                    }
                    var admin = userManager.FindByName(AppSettings.Instance.ObligatoryAdminName);
                    userManager.AddToRole(admin.Id, AppRolesHelper.RoleSystemName(AppRole.Admin));
                    _log.Debug("Obligatory admin user created successfully");
                }
            }
        }
    }
}