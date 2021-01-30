using Cat.Domain.BotApiComponents.Component;
using System;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Client
{
    public abstract class BotApiClientBase<TOperationalClient> : BotApiStatefulComponentBase, IBotApiClient<TOperationalClient>
    {
        public abstract Task RegisterClientAsync();
        public abstract Task UnregisterClientAsync();
        public TOperationalClient OperationalClient { get; protected set; }
        public virtual bool CanConsumeOperationalClient => BotApiComponentState.IsRegistered(this) && OperationalClient != null;

        #region ConsumeOperationalClient

        public void ConsumeOperationalClient(Action<TOperationalClient> action, Action handlingAction = null)
        {
            if (CanConsumeOperationalClient) action(OperationalClient);
            else handlingAction?.Invoke();
        }

        public TResult ConsumeOperationalClient<TResult>(Func<TOperationalClient, TResult> action, Func<TResult> handlingAction = null)
        {
            if (CanConsumeOperationalClient) return action(OperationalClient);
            return handlingAction != null ? handlingAction() : default;
        }

        public bool ConsumeOperationalClientConfirmable(Action<TOperationalClient> action, Action handlingAction = null)
        {
            if (CanConsumeOperationalClient)
            {
                action(OperationalClient);
                return true;
            }
            handlingAction?.Invoke();
            return false;
        }

        public Task ConsumeOperationalClientAsync(Func<TOperationalClient, Task> actionAsync) =>
            CanConsumeOperationalClient ? actionAsync(OperationalClient) : Task.CompletedTask;

        public Task ConsumeOperationalClientAsync(Func<TOperationalClient, Task> actionAsync, Func<Task> handlingActionAsync) =>
            CanConsumeOperationalClient ? actionAsync(OperationalClient) : handlingActionAsync();

        public Task ConsumeOperationalClientAsync(Func<TOperationalClient, Task> actionAsync, Action handlingAction)
        {
            if (CanConsumeOperationalClient) return actionAsync(OperationalClient);
            handlingAction();
            return Task.CompletedTask;
        }

        public Task<TResult> ConsumeOperationalClientAsync<TResult>(Func<TOperationalClient, Task<TResult>> actionAsync) =>
            CanConsumeOperationalClient ? actionAsync(OperationalClient) : default;

        public Task<TResult> ConsumeOperationalClientAsync<TResult>(Func<TOperationalClient, Task<TResult>> actionAsync, Func<Task<TResult>> handlingActionAsync) =>
            CanConsumeOperationalClient ? actionAsync(OperationalClient) : handlingActionAsync();

        public Task<TResult> ConsumeOperationalClientAsync<TResult>(Func<TOperationalClient, Task<TResult>> actionAsync, Func<TResult> handlingAction) =>
            CanConsumeOperationalClient ? actionAsync(OperationalClient) : Task.FromResult(handlingAction());

        public async Task<bool> ConsumeOperationalClientConfirmableAsync(Func<TOperationalClient, Task> actionAsync, Func<Task> handlingActionAsync)
        {
            if (CanConsumeOperationalClient)
            {
                await actionAsync(OperationalClient);
                return true;
            }
            await handlingActionAsync();
            return false;
        }

        public async Task<bool> ConsumeOperationalClientConfirmableAsync(Func<TOperationalClient, Task> actionAsync, Action handlingAction)
        {
            if (CanConsumeOperationalClient)
            {
                await actionAsync(OperationalClient);
                return true;
            }
            handlingAction();
            return false;
        }

        #endregion

    }
}
