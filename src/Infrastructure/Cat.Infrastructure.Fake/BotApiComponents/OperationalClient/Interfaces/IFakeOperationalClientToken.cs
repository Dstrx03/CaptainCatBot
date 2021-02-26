namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces
{
    public interface IFakeOperationalClientToken
    {
        bool IsTokenValid(string token);
    }
}
