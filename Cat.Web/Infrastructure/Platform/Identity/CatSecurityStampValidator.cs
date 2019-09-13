using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Cat.Web.Infrastructure.Platform.Identity
{
    /// <summary>
    /// Static helper class used to configure a CookieAuthenticationProvider to validate a cookie against a user security stamp
    /// </summary>
    public static class CatSecurityStampValidator
    {
        /// <summary>
        /// Can be used as the ValidateIdentity method for a CookieAuthenticationProvider which will check a user security stamp after validateInterval
        /// Rejects the identity if the stamp changes, and otherwise will call regenerateIdentity to sign in a new ClaimsIdentity
        /// </summary>
        /// <typeparam name="TManager"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="validateInterval"></param>
        /// <param name="regenerateIdentity"></param>
        /// <returns></returns>
        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity<TManager, TUser>(TimeSpan validateInterval, Func<TManager, TUser, Task<ClaimsIdentity>> regenerateIdentity)
            where TManager : UserManager<TUser, string>
            where TUser : class, IUser<string>
        {
            return OnValidateIdentity<TManager, TUser, string>(validateInterval, regenerateIdentity, (id) => id.GetUserId());
        }

        /// <summary>
        /// Can be used as the ValidateIdentity method for a CookieAuthenticationProvider which will check a user security stamp after validateInterval
        /// Rejects the identity if the stamp changes, and otherwise will call regenerateIdentity to sign in a new ClaimsIdentity
        /// </summary>
        /// <typeparam name="TManager"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="validateInterval"></param>
        /// <param name="regenerateIdentityCallback"></param>
        /// <param name="getUserIdCallback"></param>
        /// <returns></returns>
        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity<TManager, TUser, TKey>(TimeSpan validateInterval, Func<TManager, TUser, Task<ClaimsIdentity>> regenerateIdentityCallback, Func<ClaimsIdentity, TKey> getUserIdCallback)
            where TManager : UserManager<TUser, TKey>
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
        {
            return async (context) =>
            {
                DateTimeOffset currentUtc = DateTimeOffset.UtcNow;
                if (context.Options != null && context.Options.SystemClock != null)
                {
                    currentUtc = context.Options.SystemClock.UtcNow;
                }
                DateTimeOffset? issuedUtc = context.Properties.IssuedUtc;

                // Only validate if enough time has elapsed
                bool validate = (issuedUtc == null);
                if (issuedUtc != null)
                {
                    TimeSpan timeElapsed = currentUtc.Subtract(issuedUtc.Value);
                    validate = timeElapsed > validateInterval;
                }
                if (validate)
                {
                    var manager = context.OwinContext.GetUserManager<TManager>();
                    var userId = getUserIdCallback(context.Identity);
                    if (manager != null && userId != null)
                    {
                        var user = await manager.FindByIdAsync(userId).ConfigureAwait(false);
                        bool reject = true;
                        // Refresh the identity if the stamp matches, otherwise reject
                        if (user != null && manager.SupportsUserSecurityStamp)
                        {
                            string securityStamp = context.Identity.FindFirstValue(Constants.DefaultSecurityStampClaimType);
                            if (securityStamp == await manager.GetSecurityStampAsync(userId).ConfigureAwait(false))
                            {
                                reject = false;
                                // Regenerate fresh claims if possible and resign in
                                if (regenerateIdentityCallback != null)
                                {
                                    ClaimsIdentity identity = await regenerateIdentityCallback.Invoke(manager, user);
                                    if (identity != null)
                                    {
                                        var isPersistent = context.Properties.IsPersistent;
                                        AuthenticationProperties prop = new AuthenticationProperties();
                                        prop.AllowRefresh = true; //without this, will log out after 30 minutes
                                        prop.IsPersistent = isPersistent; //without this, will log out after 30 minutes, or whenever the browser session is ended
                                        context.OwinContext.Authentication.SignIn(prop, identity);
                                    }
                                }
                            }
                        }
                        if (reject)
                        {
                            context.RejectIdentity();
                            context.OwinContext.Authentication.SignOut(context.Options.AuthenticationType);
                        }
                    }
                }
            };
        }
    }
}