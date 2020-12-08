
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
            Error = 3,
            Unknown = 4
        }

        #region Static members

        #region Registered

        public static BotApiComponentState CreateRegistered(string description = null) => 
            new BotApiComponentState(Value.Registered, description);

        public static bool IsRegistered(IBotApiComponent botApiComponent) => 
            IsInState(botApiComponent, Value.Registered);

        #endregion

        #region Unregistered

        public static BotApiComponentState CreateUnregistered(string description = null) => 
            new BotApiComponentState(Value.Unregistered, description);

        public static bool IsUnregistered(IBotApiComponent botApiComponent) => 
            IsInState(botApiComponent, Value.Unregistered);

        #endregion

        #region Error

        public static BotApiComponentState CreateError(string description = null) => 
            new BotApiComponentState(Value.Error, description);

        public static bool IsError(IBotApiComponent botApiComponent) => 
            IsInState(botApiComponent, Value.Error);

        #endregion

        #region Unknown

        public static BotApiComponentState CreateUnknown(string description = null) =>
            new BotApiComponentState(Value.Unknown, description);

        public static bool IsUnknown(IBotApiComponent botApiComponent) => 
            IsInState(botApiComponent, Value.Unknown);

        #endregion

        private static bool IsInState(IBotApiComponent botApiComponent, Value state) => 
            botApiComponent.ComponentState.State == state;

        #endregion
    }

    public static class BotApiComponentStateExtensions // todo: names, place & design for extension methods
    {
        public static string FooBar(this BotApiComponentState source) // todo: formatting components
        {
            return string.IsNullOrEmpty(source.Description) ? source.State.ToString() : $"{source.State} - {source.Description}";
        }
    }
}
