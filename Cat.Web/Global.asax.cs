using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Cat.Domain;
using Cat.Web.App_Start;
using Cat.Web.Infrastructure.Platform;
using log4net;

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

        protected void Application_Start()
        {
            _log.Debug("Application started...");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End(object sender, EventArgs e)
        {
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
                throw;
            }
            catch (Exception ex)
            {
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
