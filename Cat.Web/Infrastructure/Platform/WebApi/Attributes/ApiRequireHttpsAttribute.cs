using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Cat.Web.Infrastructure.Platform.WebApi.Attributes
{
    public class ApiRequireHttpsAttribute : AuthorizationFilterAttribute
    {

        public ApiRequireHttpsAttribute(string[] allowedPaths)
        {
            AllowedPaths = allowedPaths;
        }

        public string[] AllowedPaths { get; private set; }


        public override void OnAuthorization(HttpActionContext actionContext)  
        {  
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps && !IsAllowedHttpPath(actionContext.Request.RequestUri.AbsolutePath))  
            {
                actionContext.Response = new HttpResponseMessage (System.Net.HttpStatusCode.Forbidden)  
                {  
                    ReasonPhrase = "HTTPS Required for this call"  
                };  
            }  
            else  
            {    
                base.OnAuthorization(actionContext);  
            }  
        }

        private bool IsAllowedHttpPath(string path)
        {
            return AllowedPaths.Any(allowedPath => path.ToLowerInvariant().Trim().Contains(allowedPath.ToLowerInvariant().Trim()));
        }
    } 
}