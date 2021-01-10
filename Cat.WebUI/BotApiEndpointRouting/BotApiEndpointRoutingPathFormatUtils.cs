using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cat.WebUI.BotApiEndpointRouting
{
    public class BotApiEndpointRoutingPathFormatUtils
    {
        public const string ControllerNamingConventionToken = "Controller";
        public const string ControllerTemplateToken = "[controller]";
        public const string BasePathTemplate = "api/todo/" + ControllerTemplateToken;

        private string EndpointPathFormatPattern => "^(/[^/?&# ]+)+$";
        private string ControllerPathFormatPattern { get; } = $"^/{BasePathTemplate.Replace(ControllerTemplateToken, "([A-Z]+)")}(/[A-Z]+)+$";

        private readonly HashSet<string> _consumableControllerActionRoutesSet;

        public BotApiEndpointRoutingPathFormatUtils(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _consumableControllerActionRoutesSet = GetConsumableControllerActionRoutesSet(actionDescriptorCollectionProvider);
        }

        public string NormalizePath(string path) =>
            path.ToUpperInvariant();

        public string ParseControllerPathTemplate(string controllerPathTemplate, string controllerName) =>
            $"/{controllerPathTemplate.Replace(ControllerTemplateToken, controllerName.Replace(ControllerNamingConventionToken, string.Empty))}";

        public string GetNormalizedRequestPath(HttpContext context)
        {
            var path = NormalizePath(context.Request.Path.ToUriComponent());
            return path.EndsWith('/') && path.Length > 1 ? path.Substring(0, path.Length - 1) : path;
        }

        public bool ControllerPathIsInCorrectFormat(string controllerPath) =>
            Regex.IsMatch(controllerPath, ControllerPathFormatPattern, RegexOptions.IgnoreCase);

        public bool EndpointPathIsInCorrectFormat(string endpointPath) =>
            Regex.IsMatch(endpointPath, EndpointPathFormatPattern, RegexOptions.IgnoreCase);

        public bool ControllerPathMatchesConsumableControllerActionRoute(string controllerPathNormalized) =>
            _consumableControllerActionRoutesSet.Contains(controllerPathNormalized);

        private HashSet<string> GetConsumableControllerActionRoutesSet(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) =>
            actionDescriptorCollectionProvider.ActionDescriptors.Items
                .Where(_ =>
                    _.AttributeRouteInfo?.Template != null &&
                    _.AttributeRouteInfo.Template.StartsWith(BasePathTemplate.Replace(ControllerTemplateToken, string.Empty)))
                .Select(_ => $"/{NormalizePath(_.AttributeRouteInfo.Template)}")
                .ToHashSet();
    }
}
