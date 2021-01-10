using Cat.Application;
using Cat.Domain;
using Cat.Infrastructure;
using Cat.WebUI.BotApiEndpointRouting;
using Cat.WebUI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

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

            // todo: use extension method
            services.AddSingleton<BotApiEndpointRoutingService>();
            services.AddSingleton<BotApiEndpointRoutingPathFormatUtils>();
            services.AddTransient<BotApiEndpointRoutingPathFactory>();
            services.AddSingleton<BotApiEndpointBase, FakeBotApiEndpoint>(new FakeBotApiEndpoint.Factory().Create);
            // todo: ==========================

            services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory
        /*TODO: REMOVE!*/, IEnumerable<BotApiEndpointBase> botApiEndpoints, FakeBotApiClient fakeClient, FakeBotApiWebhook fakeWebhook, FakeBotApiPoller fakePoller/*TODO: REMOVE!*/)
        {
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

            app.UseBotApiEndpointRouting();

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

            applicationLifetime.Foo(loggerFactory.CreateLogger("Foo"), fakeClient, botApiEndpoints.ToList(), fakeWebhook, fakePoller); // todo: position in Configure(...) method
        }
    }



    internal static class FooBar // todo: name, location & design
    {
        internal static IHostApplicationLifetime Foo(this IHostApplicationLifetime applicationLifetime,
            ILogger logger,
            IBotApiClient<FakeOperationalClient> fakeClient,
            IList<BotApiEndpointBase> botApiEndpoints,
            IBotApiRevisableWebhook<FakeWebhookInfo> fakeWebhook,
            IBotApiPoller fakePoller)
        {
            applicationLifetime.ApplicationStarted.Register(() => ApplicationStarted(logger, fakeClient, botApiEndpoints, fakeWebhook, fakePoller));
            applicationLifetime.ApplicationStopping.Register(() => ApplicationStopping(logger));
            applicationLifetime.ApplicationStopped.Register(() => ApplicationStopped(logger));

            return applicationLifetime;
        }

        private static async void ApplicationStarted(ILogger logger,
            IBotApiClient<FakeOperationalClient> fakeClient,
            IList<BotApiEndpointBase> botApiEndpoints,
            IBotApiRevisableWebhook<FakeWebhookInfo> fakeWebhook,
            IBotApiPoller fakePoller)
        {
            logger.LogInformation("Cat started!");

            // todo: ensure all component which may be possibly used in concurrent context are thread safe (FakeBotApiWebhook definitely not thread safe)
            // todo: move registration of endpoints to IBotApiComponentsManager
            await fakeClient.RegisterClientAsync();
            foreach (var botApiEndpoint in botApiEndpoints)
            {
                botApiEndpoint.RegisterEndpoint();
            }
            await fakeWebhook.RegisterWebhookAsync();
            fakePoller.RegisterPoller();
            // todo: ===========================================================

            PrintSmth(logger, fakeClient, botApiEndpoints, fakeWebhook, fakePoller);

            logger.LogInformation("Cat started! (completed)");
        }

        /*todo: REMOVE*/
        private static int c = 1;
        private static void PrintSmth(ILogger logger,
            IBotApiClient<FakeOperationalClient> fakeClient,
            IList<BotApiEndpointBase> _botApiEndpoints,
            IBotApiRevisableWebhook<FakeWebhookInfo> fakeWebhook,
            IBotApiPoller fakePoller)
        {
            logger.LogDebug($"[print start #{c}]*******************************");
            logger.LogDebug($"client: [{fakeClient.ComponentState.State}] {fakeClient.ComponentState.Description}");
            logger.LogDebug($"endpoint: [{_botApiEndpoints.First().ComponentState.State}] {_botApiEndpoints.First().ComponentState.Description}");
            logger.LogDebug($"webhook: [{fakeWebhook.ComponentState.State}] {fakeWebhook.ComponentState.Description} whInfo: '{fakeWebhook.WebhookInfo?.Url}', {fakeWebhook.WebhookInfo?.Date}");
            logger.LogDebug($"poller: [{fakePoller.ComponentState.State}] {fakePoller.ComponentState.Description}");
            logger.LogDebug($"*******************************[print end #{c}]");
            c++;
        }
        /*todo: REMOVE*/

        private static void ApplicationStopping(ILogger logger)
        {
            logger.LogInformation("Cat stopping...");
        }

        private static void ApplicationStopped(ILogger logger)
        {
            logger.LogInformation("Cat stopped!");
        }
    }



}
