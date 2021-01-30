namespace Cat.Domain.BotApiComponents.Component
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
            Error = 3,
            Unknown = 4
        }

        #region Static members

        #region Registered

        public static BotApiComponentState CreateRegistered(string description = null) =>
            new BotApiComponentState(Value.Registered, description);

        public static bool IsRegistered(IBotApiStatefulComponent botApiComponent) =>
            IsInState(botApiComponent, Value.Registered);

        #endregion

        #region Unregistered

        public static BotApiComponentState CreateUnregistered(string description = null) =>
            new BotApiComponentState(Value.Unregistered, description);

        public static bool IsUnregistered(IBotApiStatefulComponent botApiComponent) =>
            IsInState(botApiComponent, Value.Unregistered);

        #endregion

        #region Error

        public static BotApiComponentState CreateError(string description = null) =>
            new BotApiComponentState(Value.Error, description);

        public static bool IsError(IBotApiStatefulComponent botApiComponent) =>
            IsInState(botApiComponent, Value.Error);

        #endregion

        #region Unknown

        public static BotApiComponentState CreateUnknown(string description = null) =>
            new BotApiComponentState(Value.Unknown, description);

        public static bool IsUnknown(IBotApiStatefulComponent botApiComponent) =>
            IsInState(botApiComponent, Value.Unknown);

        #endregion

        private static bool IsInState(IBotApiStatefulComponent botApiComponent, Value state) =>
            botApiComponent.ComponentState.State == state;

        #endregion
    }
}
