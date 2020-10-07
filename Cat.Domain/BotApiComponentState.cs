
namespace Cat.Domain
{
    public class BotApiComponentState
    {
        private BotApiComponentState(Value state, string description)
        {
            State = state;
            Description = description;
        }

        public Value State { get; }

        public string Description { get; }

        public enum Value
        {
            Registered = 1,
            Unregistered = 2,
            Error = 3
        }

        #region Static members

        #region Registered

        public static BotApiComponentState CreateRegistered(string description = null)
        {
            return new BotApiComponentState(Value.Registered, description);
        }

        public static bool IsRegistered(IBotApiComponent botApiComponent)
        {
            return IsInState(botApiComponent, Value.Registered);
        }

        #endregion

        #region Unregistered

        public static BotApiComponentState CreateUnregistered(string description = null)
        {
            return new BotApiComponentState(Value.Unregistered, description);
        }

        public static bool IsUnregistered(IBotApiComponent botApiComponent)
        {
            return IsInState(botApiComponent, Value.Unregistered);
        }

        #endregion

        #region Error

        public static BotApiComponentState CreateError(string description = null)
        {
            return new BotApiComponentState(Value.Error, description);
        }

        public static bool IsError(IBotApiComponent botApiComponent)
        {
            return IsInState(botApiComponent, Value.Error);
        }

        #endregion

        private static bool IsInState(IBotApiComponent botApiComponent, Value state)
        {
            return botApiComponent.ComponentState.State == state;
        }

        #endregion
    }
}
