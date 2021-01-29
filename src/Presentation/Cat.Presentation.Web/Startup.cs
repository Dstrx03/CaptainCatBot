using Cat.Infrastructure.Services;
using Cat.Presentation.BotApiEndpointRouting.Middlewares;
using Cat.Presentation.BotApiEndpointRouting.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cat.Presentation.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDomainBotUpdates();
            services.AddApplication();
            services.AddInfrastructureFake();
            services.AddPresentationWeb();
            services.AddPresentationBotApiEndpointRouting();

            services.AddHostedServicesQueued(WebHostEnvironment);

            services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (WebHostEnvironment.IsDevelopment())
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
            if (!WebHostEnvironment.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseBotApiEndpointRouting(endpoints =>
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

                if (WebHostEnvironment.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }

    internal static class StartupExtensions
    {
        internal static IServiceCollection AddHostedServicesQueued(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            services.AddBotApiEndpointRoutingInitializer(configuration =>
            {
                configuration.CheckControllersCovered = webHostEnvironment.IsDevelopment();
            });
            services.AddHostedService<BotApiComponentsLifetimeManager>();

            return services;
        }
    }
}
