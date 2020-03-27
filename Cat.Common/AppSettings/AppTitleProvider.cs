
using System.Web.Configuration;

namespace Cat.Common.AppSettings
{
    public class AppTitleProvider
    {
        public static string AppTitle
        {
            get
            {
                return WebConfigurationManager.AppSettings["AppTitle"];
            }
        }

        public static string AppVersion
        {
            get
            {
                return WebConfigurationManager.AppSettings["AppVersion"];
            }
        }

        public static string AppTitleFullInternalFormat
        {
            get { return string.Format("{0} | CaptainCatBot | v{1}", AppTitle, AppVersion); }
        }
    }
}