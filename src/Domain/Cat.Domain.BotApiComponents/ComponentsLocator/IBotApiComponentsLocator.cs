using Cat.Domain.BotApiComponents.Component;
using System.Collections.Generic;
using System.Linq;

namespace Cat.Domain.BotApiComponents.ComponentsLocator
{
    public interface IBotApiComponentsLocator
    {
        IEnumerable<IBotApiComponent> Components { get; }

        T GetComponentByDescriptor<T>(BotApiComponentDescriptor componentDescriptor) where T : IBotApiComponent =>
           (T)Components.SingleOrDefault(_ => ComponentIsCompatibleWithType<T>(_) && ComponentAppliesDescriptor(_, componentDescriptor));

        IEnumerable<IBotApiComponent> GetAllComponentsByDescriptor(BotApiComponentDescriptor componentDescriptor) =>
            Components.Where(_ => ComponentAppliesDescriptor(_, componentDescriptor));

        IEnumerable<T> GetAllComponents<T>() where T : IBotApiComponent =>
            Components.Where(ComponentIsCompatibleWithType<T>).Cast<T>();

        private bool ComponentIsCompatibleWithType<T>(IBotApiComponent component) where T : IBotApiComponent =>
            component is T;

        private bool ComponentAppliesDescriptor(IBotApiComponent component, BotApiComponentDescriptor descriptor) =>
            component.ComponentDescriptor == descriptor;
    }
}
