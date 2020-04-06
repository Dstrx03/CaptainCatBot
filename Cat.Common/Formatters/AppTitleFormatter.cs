using Cat.Common.AppSettings.Providers;

namespace Cat.Common.Formatters
{
    public interface IAppTitleFormatter
    {
        string AppTitleFullInternalFormat { get; }
    }

    internal class AppTitleFormatter : IAppTitleFormatter
    {
        public string AppTitleFullInternalFormat
        {
            get { return string.Format("{0} | CaptainCatBot | v{1}", AppSettings.AppSettings.Instance.AppTitle, AppSettings.AppSettings.Instance.AppVersion); }
        }
    }
}
