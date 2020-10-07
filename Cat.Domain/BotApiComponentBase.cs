
namespace Cat.Domain
{
    public abstract class BotApiComponentBase : IBotApiComponent
    {
        protected BotApiComponentBase()
        {
            InitComponentState();
        }

        protected virtual void InitComponentState()
        {
            ComponentState = BotApiComponentState.CreateUnregistered();
        }

        public BotApiComponentState ComponentState { get; protected set; }
    }
}
