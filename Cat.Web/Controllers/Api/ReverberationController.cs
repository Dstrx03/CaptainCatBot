﻿using System.Web.Http;
using Cat.Business.Services.SystemLogging;
using Cat.Business.Services.SystemLogging.Factory;
using Cat.Common.Formatters;
using Cat.Common.Helpers;
using log4net;
using StructureMap;

namespace Cat.Web.Controllers.Api
{
    public class ReverberationController : ApiController
    {
        private readonly IContainer _container;
        private readonly IAppTitleFormatter _appTitleFormatter;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReverberationController(IContainer container, IAppTitleFormatter appTitleFormatter)
        {
            _container = container;
            _appTitleFormatter = appTitleFormatter;
        }

        [HttpGet]
        public string Refresher()
        {
            return MaskDataHelper.MoshText(string.Format("'{0}' app Refresher Service endpoint", _appTitleFormatter.AppTitleFullInternalFormat));
        }

        [HttpGet]
        public void ATrigger(string message)
        {
            var loggingService = SystemLoggingServiceFactory.CreateService(ServiceType.ATrigger, _container);
            var reverbMsg = string.Format("Received request on ATrigger Service endpoint, message: '{0}'", message);
            loggingService.AddEntry(reverbMsg);
            _log.Debug(reverbMsg);
        }

    }
}
