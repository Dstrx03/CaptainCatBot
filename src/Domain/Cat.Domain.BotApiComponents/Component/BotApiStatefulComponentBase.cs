namespace Cat.Domain.BotApiComponents.Component
{
    public abstract class BotApiStatefulComponentBase : IBotApiStatefulComponent
    {
        private BotApiComponentState _componentState;

        protected virtual BotApiComponentState DefaultComponentState { get; } = BotApiComponentState.CreateUnregistered();

        public abstract BotApiComponentDescriptor ComponentDescriptor { get; }

        public BotApiComponentState ComponentState
        {
            get => _componentState ??= DefaultComponentState;
            protected set => _componentState = value ?? DefaultComponentState;
        }
    }
}
