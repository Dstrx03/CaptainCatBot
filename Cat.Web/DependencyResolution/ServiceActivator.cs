using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Cat.Web.App_Start;

namespace Cat.Web.DependencyResolution
{
    public class ServiceActivator : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return StructuremapMvc.StructureMapDependencyScope.Container.GetInstance(controllerType) as IHttpController;
        }
    }
}