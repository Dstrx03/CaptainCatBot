using System.Collections.Generic;
using Cat.Domain;

namespace Cat.WebUI.BotApiEndpoints
{
    public abstract class BotApiEndpointBase : BotApiComponentBase, IBotApiEndpoint
    {
        public virtual void RegisterEndpoint()
        {
            ComponentState = BotApiComponentState.CreateRegistered();
        }

        public virtual void UnregisterEndpoint()
        {
            ComponentState = BotApiComponentState.CreateUnregistered();
        }

        public IEnumerable<BotApiEndpointPath?> Paths { get; private set; } // todo: maybe better names BotApiEndpointPath/Paths ???

        protected void SetPaths(IEnumerable<BotApiEndpointPath?> paths)
        {
            if (paths != null) Paths = paths;
        }

        protected BotApiEndpointPath? CreatePath(string controllerPathTemplate, string controllerName, string endpointPath)
        {
            var controllerPath = controllerPathTemplate.Replace("[controller]", controllerName.Replace("Controller", string.Empty));
            return new BotApiEndpointPath(controllerPath, endpointPath);
        }
    }
}
