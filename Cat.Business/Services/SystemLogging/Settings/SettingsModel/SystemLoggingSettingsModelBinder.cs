using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Cat.Business.Services.SystemLogging.Settings.SettingsBuilder;
using Newtonsoft.Json;

namespace Cat.Business.Services.SystemLogging.Settings.SettingsModel
{
    public class SystemLoggingSettingsModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(List<SystemLoggingSettings>)) return false;

            try
            {
                var settingsListJson = GetRequestBodyString(actionContext);
                var settingsListRaw = JsonConvert.DeserializeObject<List<SystemLoggingSettings>>(settingsListJson);
                var settingsList = settingsListRaw.Select(m => new FromSettingsSettingsBuilder(m)).Select(builder => builder.GetResult()).ToList();
                bindingContext.Model = settingsList;
                return true;
            }
            catch (Exception e)
            {
                bindingContext.ModelState.AddModelError("Exception", e);
                return false;
            }
        }

        // TODO: Move to Cat.Common for dedicated helper?
        private string GetRequestBodyString(HttpActionContext actionContext)
        {
            using (var stream = new MemoryStream())
            {
                var context = (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];
                context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                context.Request.InputStream.CopyTo(stream);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
