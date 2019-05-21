
using Microsoft.AspNet.Identity.Owin;

namespace Cat.Web.Models.Identity
{
    public class LoginResult
    {
        public SignInStatus SignInStatus { get; set; }

        public string [] Errors { get; set; }
    }
}