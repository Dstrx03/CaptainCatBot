
namespace Cat.WebUI.BotApiEndpointRouting
{
    public struct BotApiEndpointRoutingPath
    {
        public string ControllerPath { get; init; }
        public string ControllerPathNormalized { get; init; }
        public string EndpointPath { get; init; }
        public string EndpointPathNormalized { get; init; }
    }

    public static class BotApiEndpointRoutingPathExtensions
    {
        public static string Details(this BotApiEndpointRoutingPath source)
        {
            // todo: move to messages formatting service
            return $"ControllerPath: '{source.ControllerPath}', ControllerPathNormalized: '{source.ControllerPathNormalized}', EndpointPath: '{source.EndpointPath}', EndpointPathNormalized: '{source.EndpointPathNormalized}'";
        }
    }
}
