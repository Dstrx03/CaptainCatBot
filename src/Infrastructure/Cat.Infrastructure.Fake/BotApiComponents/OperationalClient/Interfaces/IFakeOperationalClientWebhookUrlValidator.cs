using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces
{
    public interface IFakeOperationalClientWebhookUrlValidator
    {
        Task ValidateWebhookUrlAsync(string webhookUrl);
        void ConfirmWebhookUrlValidationToken(string validationToken, string webhookUrl);
    }
}
