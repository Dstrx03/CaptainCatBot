using System;
using System.Collections.Generic;
using System.Linq;
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
                var settingsListJson = actionContext.Request.Content.ReadAsStringAsync().Result;
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
    }
}
