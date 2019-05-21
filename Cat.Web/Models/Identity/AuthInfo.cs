
namespace Cat.Web.Models.Identity
{
    public class AuthInfo
    {
        public bool IsAuthenticated { get; set; }

        public AuthUserInfo AuthUserInfo { get; set; }
    }

    public class AuthUserInfo
    {
        public string Name { get; set; }
    }
}