namespace Cat.Domain.BotApiComponents.Component
{
    public abstract class BotApiComponentBase : IBotApiComponent
    {
        private BotApiComponentState _componentState;

        protected virtual BotApiComponentState DefaultComponentState { get; } = BotApiComponentState.CreateUnregistered();

        public BotApiComponentState ComponentState
        {
            get => _componentState ??= DefaultComponentState;
            protected set => _componentState = value ?? DefaultComponentState;
        }
    }
}
