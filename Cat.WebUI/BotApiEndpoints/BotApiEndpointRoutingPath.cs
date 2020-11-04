
namespace Cat.WebUI.BotApiEndpoints
{
    public struct BotApiEndpointRoutingPath
    {
        public BotApiEndpointRoutingPath(string controllerPath, string endpointPath)
        {
            ControllerPath = controllerPath;
            EndpointPath = endpointPath;
            ControllerPathNormalized = controllerPath.ToUpperInvariant();
            EndpointPathNormalized = endpointPath.ToUpperInvariant();
        }

        public string ControllerPath { get; }
        public string ControllerPathNormalized { get; }
        public string EndpointPath { get; }
        public string EndpointPathNormalized { get; }
    }
}
