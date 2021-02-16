using Cat.Domain.BotApiComponents.Component;
using Cat.Domain.BotApiComponents.ComponentsLocator;
using Cat.Domain.BotApiComponents.ComponentsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.ComponentsLifetimeManager
{
    public abstract class BotApiComponentsLifetimeManagerBase
    {
        // todo: ensure all components which may be possibly used in concurrent context are thread safe (FakeBotApiWebhook definitely not thread safe)

        protected virtual async Task ApplyRegisterManagersComponentsAsync(
            IEnumerable<IBotApiComponentsManager> botApiComponentsManagers,
            IBotApiComponentsLocator botApiComponentsLocator)
        {
            foreach (var componentsManager in botApiComponentsManagers)
                await componentsManager.RegisterComponentsAsync(botApiComponentsLocator);
        }

        protected virtual async Task ApplyUnregisterManagersComponentsAsync(
            IEnumerable<IBotApiComponentsManager> botApiComponentsManagers,
            IBotApiComponentsLocator botApiComponentsLocator)
        {
            foreach (var componentsManager in botApiComponentsManagers)
                await componentsManager.UnregisterComponentsAsync(botApiComponentsLocator);
        }

        protected IEnumerable<IBotApiComponentsManager> GetComponentsManagers(IBotApiComponentsLocator botApiComponentsLocator) =>
           botApiComponentsLocator.GetAllComponents<IBotApiComponentsManager>();

        protected IEnumerable<IBotApiComponentsManager> GetComponentsManagersToRegister(IBotApiComponentsLocator botApiComponentsLocator) =>
           GetComponentsManagers(botApiComponentsLocator).Where(_ => _.RegisterAtApplicationStart && !_.RegisterAtApplicationStartAfterAppHost);

        protected IEnumerable<IBotApiComponentsManager> GetComponentsManagersToRegisterAfterAppHost(IBotApiComponentsLocator botApiComponentsLocator) =>
           GetComponentsManagers(botApiComponentsLocator).Where(_ => _.RegisterAtApplicationStart && _.RegisterAtApplicationStartAfterAppHost);

        protected void CheckComponentsSetup(IBotApiComponentsLocator botApiComponentsLocator)
        {
            CheckAllComponentsHaveDescriptorSpecified(botApiComponentsLocator);
            CheckAllComponentsHaveNoConflictsWithinGroup(botApiComponentsLocator);
            CheckAllComponentGroupsIncludeComponentsManager(botApiComponentsLocator);
            // todo: check that components set can be used at least by one "management strategy" (when Strategies will be implemented)?
        }

        protected void CheckAllComponentsHaveDescriptorSpecified(IBotApiComponentsLocator botApiComponentsLocator)
        {
            var componentsNames = GetInconsistentComponentsHavingDescriptorUnspecified(botApiComponentsLocator)
                .Select(_ => _.GetType().Name)
                .ToList();
            if (componentsNames.Any())
                throw new InvalidOperationException($"The following Bot API Component(s) have unspecified Descriptor: {componentsNames.BarEnumerableToStrFmt()}.");
        }

        private IEnumerable<IBotApiComponent> GetInconsistentComponentsHavingDescriptorUnspecified(IBotApiComponentsLocator botApiComponentsLocator) =>
            botApiComponentsLocator.GetAllComponentsByDescriptor(BotApiComponentDescriptor.None);

        protected void CheckAllComponentsHaveNoConflictsWithinGroup(IBotApiComponentsLocator botApiComponentsLocator)
        {
            var componentGroupsNames = GetInconsistentComponentsConflictingWithinGroup(botApiComponentsLocator)
                .GroupBy(_ => _.ComponentDescriptor, _ => _.GetType().Name)
                .Select(_ => $"{_.Key} ({string.Join(", ", _)})")
                .ToList();
            if (componentGroupsNames.Any())
                throw new InvalidOperationException($"The following Bot API Component group(s) have conflicting Components: {componentGroupsNames.BarEnumerableToStrFmt()}.");
        }

        private IEnumerable<IBotApiComponent> GetInconsistentComponentsConflictingWithinGroup(IBotApiComponentsLocator botApiComponentsLocator)
        {
            var conflictingComponentsSet = new HashSet<IBotApiComponent>();
            foreach (var descriptor in IBotApiComponent.ApplicableComponentDescriptors)
            {
                var encounteredInterfacesSignaturesDictionary = new Dictionary<string, IBotApiComponent>();
                foreach (var component in botApiComponentsLocator.GetAllComponentsByDescriptor(descriptor))
                {
                    var interfacesOrdered = component.GetType()
                        .GetInterfaces()
                        .Where(_ => _.IsAssignableTo(typeof(IBotApiComponent)))
                        .Select(_ => _.Name)
                        .OrderBy(_ => _);
                    var interfacesSignature = string.Join('_', interfacesOrdered);

                    if (encounteredInterfacesSignaturesDictionary.TryAdd(interfacesSignature, component)) continue;

                    encounteredInterfacesSignaturesDictionary.TryGetValue(interfacesSignature, out var alreadyStoredComponent);
                    conflictingComponentsSet.Add(alreadyStoredComponent);
                    conflictingComponentsSet.Add(component);
                }
            }
            return conflictingComponentsSet;
        }

        protected void CheckAllComponentGroupsIncludeComponentsManager(IBotApiComponentsLocator botApiComponentsLocator)
        {
            var descriptorsNames = GetDescriptorsMissingComponentsManagerWithinGroup(botApiComponentsLocator)
                .Select(_ => _.ToString())
                .ToList();
            if (descriptorsNames.Any())
                throw new InvalidOperationException($"The following Bot API Component group(s) do not include Bot API Components Manager: {descriptorsNames.BarEnumerableToStrFmt()}.");
        }

        private IEnumerable<BotApiComponentDescriptor> GetDescriptorsMissingComponentsManagerWithinGroup(IBotApiComponentsLocator botApiComponentsLocator)
        {
            var descriptorsSet = new HashSet<BotApiComponentDescriptor>();
            foreach (var descriptor in IBotApiComponent.ApplicableComponentDescriptors)
                if (botApiComponentsLocator.GetComponentByDescriptor<IBotApiComponentsManager>(descriptor) == null)
                    descriptorsSet.Add(descriptor);
            return descriptorsSet;
        }
    }
}
