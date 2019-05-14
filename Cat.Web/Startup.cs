﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cat.Web.Startup))]
namespace Cat.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            HangfireConfig.RegisterHangfire(app);
        }
    }
}
