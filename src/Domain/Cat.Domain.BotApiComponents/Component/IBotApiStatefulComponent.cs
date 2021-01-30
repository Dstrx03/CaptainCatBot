namespace Cat.Domain.BotApiComponents.Component
{
    public interface IBotApiStatefulComponent : IBotApiComponent
    {
        BotApiComponentState ComponentState { get; }
    }
}
