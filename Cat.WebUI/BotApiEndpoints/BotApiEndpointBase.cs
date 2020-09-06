using Cat.Domain;

namespace Cat.WebUI.BotApiEndpoints
{
    public abstract class BotApiEndpointBase : IBotApiEndpoint
    {
        protected bool _isRegistered;
        private string _botApiEndpointPath;

        protected BotApiEndpointBase(string controllerPathTemplate, string controllerName)
        {
            _isRegistered = false;
            ControllerPath = controllerPathTemplate.Replace("[controller]", controllerName.Replace("Controller", string.Empty)).ToLower();
            EndpointPath = null;
        }

        public string ControllerPath { get; }

        public string EndpointPath
        {
            get => _botApiEndpointPath;
            protected set => _botApiEndpointPath = value?.ToLower();
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
