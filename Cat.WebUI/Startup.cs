using System;
using Cat.Application;
using Cat.Domain;
using Cat.Infrastructure;
using Cat.WebUI.BotApiEndpoints;
using Cat.WebUI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cat.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDomain();
            services.AddApplication();
            services.AddInfrastructure();

            // todo: use Lamar/StructureMap with default conventions, lifetimes etc.
            services.AddSingleton<BotApiEndpointBase, FakeBotApiEndpoint>();
            // todo: ==========================

            services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseMiddleware<BotApiEndpointRoutingMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });


            // todo: think about appropriate place for code below
            var logger = loggerFactory.CreateLogger(typeof(Startup)); // todo: proper logger injection
            applicationLifetime.ApplicationStarted.Register(() => logger.LogInformation("Cat started!"));
            applicationLifetime.ApplicationStopping.Register(() => logger.LogInformation("Cat stopping..."));
            applicationLifetime.ApplicationStopped.Register(() => logger.LogInformation("Cat stopped!"));
            // todo: =============================================

            // todo: uncaught exceptions handling
            // todo: once again, think about exceptions handling in Fake Bot API Components (unlikely, exceptions only on invalid client state)
        }
    }
}
