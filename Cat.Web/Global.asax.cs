using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Cat.Business.Services.InternalServices;
using Cat.Common.AppSettings;
using Cat.Common.AppSettings.Providers;
using Cat.Common.Helpers;
using Cat.Domain;
using Cat.Web.App_Start;
using log4net;
using Microsoft.AspNet.SignalR;

namespace Cat.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /**
         * <summary>
         * Each HTTP request should use it's own context (Unit Of Work).
         * </summary>
         */
        public static DbContext DbContextPerHttpRequest
        {
            get
            {
                if (HttpContext.Current == null)
                    return new AppDbContext();

                if (CurrentDbContext == null)
                    CurrentDbContext = new AppDbContext();

                return CurrentDbContext;
            }
        }

        public static DbContext CurrentDbContext
        {
            get { return HttpContext.Current.Items["current.dbContext"] as DbContext; }
            set { HttpContext.Current.Items["current.dbContext"] = value; }
        }


        #region Framework native methods

        protected async void Application_Start()
        {
            _log.Debug("Application started...");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalHost.HubPipeline.RequireAuthentication();
            RefresherConfig.Register();
            await TelegramBotConfig.RegisterAsync();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _log.Debug("Application is forced to stop...");

            AsyncHelper.RunSync(() => ATriggerService.CallATriggerAsync(StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer(), 
                BaseUrlProvider.HttpBaseUrl, AppSettings.Instance.ATriggerApiKey, AppSettings.Instance.ATriggerApiSecret));
            AsyncHelper.RunSync(TelegramBotConfig.UnregisterAsync);

            _log.Debug("Application stopped.");

            // When application terminates it's better to wait synchronously for completion of async tasks. 
            // Using async/await pattern here may cause background threads to terminate before they complete execution.
            // TODO: Check performance.
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            StructuremapMvc.CreateNestedContainer();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            CloseContext();
            StructuremapMvc.DisposeNestedContainer();
        }

        #endregion


        /**
         *<summary>
         * Saves changes in current context and disposes it (Unit Of Work mechanics).
         *</summary>
         */
        private void CloseContext()
        {
            if (CurrentDbContext == null) return;

            try
            {
                CurrentDbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                _log.ErrorFormat("Error occured while saving changes in CurrentDbContext: {0}", ex);
                throw;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error occured while saving changes in CurrentDbContext: {0}", ex);
                throw;
            }
            finally
            {
                CurrentDbContext.Dispose();
                CurrentDbContext = null;
            }
        }

    }
}
