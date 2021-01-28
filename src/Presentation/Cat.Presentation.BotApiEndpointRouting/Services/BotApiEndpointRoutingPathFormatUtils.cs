using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cat.Presentation.BotApiEndpointRouting.Services
{
    public class BotApiEndpointRoutingPathFormatUtils
    {
        public const string ControllerNamingConventionToken = "Controller";
        public const string ControllerTemplateToken = "[controller]";
        public const string BasePathTemplate = "api/todo/" + ControllerTemplateToken;

        private string EndpointPathFormatPattern => "^(/[^/?&# ]+)+$";
        private string ControllerPathFormatPattern { get; } = $"^/{BasePathTemplate.Replace(ControllerTemplateToken, "([A-Z]+)")}(/[A-Z]+)+$";

        private readonly string _basePathTemplateSignature = BasePathTemplate.Replace(ControllerTemplateToken, string.Empty);
        private readonly IEnumerable<string> _allowedHttpMethods = new[] { "POST" };
        private readonly HashSet<string> _consumableControllerActionRoutesSet;

        public BotApiEndpointRoutingPathFormatUtils(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _consumableControllerActionRoutesSet = GetConsumableControllerActionRoutesSet(actionDescriptorCollectionProvider);
        }

        public IEnumerable<string> ConsumableControllerActionRoutes => _consumableControllerActionRoutesSet;

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
                    ActionDescriptorAppliesAllowedHttpMethods(_) &&
                    ActionDescriptorAppliesBasePathTemplateSignature(_))
                .Select(_ => $"/{NormalizePath(_.AttributeRouteInfo.Template)}")
                .ToHashSet();

        private bool ActionDescriptorAppliesAllowedHttpMethods(ActionDescriptor actionDescriptor) =>
            actionDescriptor.ActionConstraints != null &&
            actionDescriptor.ActionConstraints.Any(_ =>
                _ is HttpMethodActionConstraint constraint &&
                constraint.HttpMethods.Any(_allowedHttpMethods.Contains));

        private bool ActionDescriptorAppliesBasePathTemplateSignature(ActionDescriptor actionDescriptor) =>
            actionDescriptor.AttributeRouteInfo?.Template != null &&
            actionDescriptor.AttributeRouteInfo.Template.StartsWith(_basePathTemplateSignature);
    }
}
