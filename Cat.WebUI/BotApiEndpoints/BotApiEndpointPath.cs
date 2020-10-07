
namespace Cat.WebUI.BotApiEndpoints
{
    public struct BotApiEndpointPath
    {
        public BotApiEndpointPath(string controllerPath, string endpointPath)
        {
            ControllerPath = controllerPath;
            EndpointPath = endpointPath;
            ControllerPathNormalized = controllerPath.ToLower(); // todo: ToLowerInvariant()?
            EndpointPathNormalized = endpointPath.ToLower();
        }

        public string ControllerPath { get; }
        public string ControllerPathNormalized { get; }
        public string EndpointPath { get; }
        public string EndpointPathNormalized { get; }
    }
}
