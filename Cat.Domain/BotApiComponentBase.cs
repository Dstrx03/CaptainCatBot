
namespace Cat.Domain
{
    public abstract class BotApiComponentBase : IBotApiComponent
    {
        protected BotApiComponentBase()
        {
            InitComponentState(); // todo: remove, make workaround
        }

        protected virtual void InitComponentState()
        {
            ComponentState = BotApiComponentState.CreateUnregistered();
        }

        public BotApiComponentState ComponentState { get; protected set; }
    }
}
