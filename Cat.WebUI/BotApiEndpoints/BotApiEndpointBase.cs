using Cat.Domain;

namespace Cat.WebUI.BotApiEndpoints
{
    public abstract class BotApiEndpointBase : IBotApiEndpoint
    {
        protected bool _isRegistered;
        private string _botApiEndpointRoute;

        protected BotApiEndpointBase(string controllerRouteTemplate, string controllerName)
        {
            _isRegistered = false;
            ControllerRoute = controllerRouteTemplate.Replace("[controller]", controllerName.Replace("Controller", string.Empty)).ToLower();
            EndpointRoute = null;
        }

        public string ControllerRoute { get; }

        public string EndpointRoute
        {
            get => _botApiEndpointRoute;
            protected set => _botApiEndpointRoute = value?.ToLower();
        }

        public virtual void RegisterEndpoint()
        {
            _isRegistered = true;
        }

        public virtual void UnregisterEndpoint()
        {
            _isRegistered = false;
        }

        public virtual bool GetStatus() => _isRegistered;
    }
}
