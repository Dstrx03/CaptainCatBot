using Microsoft.AspNet.SignalR;
using Owin;

namespace Cat.Web.App_Start
{
    public class SignalRConfig
    {
        public static void RegisterSignalR(IAppBuilder app)
        {
            var hubConfig = new HubConfiguration
            {
                EnableJavaScriptProxies = false
            };

            app.MapSignalR("/signalr", hubConfig);
        }
    }
}