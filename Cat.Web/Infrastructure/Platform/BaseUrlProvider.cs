
using System.Web.Configuration;

namespace Cat.Web.Infrastructure.Platform
{
    public class BaseUrlProvider
    {
        private static readonly string _baseUrlAuthority;

        static BaseUrlProvider()
        {
            _baseUrlAuthority = GetBaseUrlAuthority();
        }

        public static string HttpBaseUrl { get { return _baseUrlAuthority != null ? string.Format("http://{0}/", _baseUrlAuthority) : null; } }

        public static string HttpsBaseUrl { get { return _baseUrlAuthority != null ? string.Format("https://{0}/", _baseUrlAuthority) : null; } }

        private static string GetBaseUrlAuthority()
        {
            var baseUrlAuthority = WebConfigurationManager.AppSettings["BaseUrlAuthority"];
            return string.IsNullOrEmpty(baseUrlAuthority) ? null : baseUrlAuthority;
        }
    }
}