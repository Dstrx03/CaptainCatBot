
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
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