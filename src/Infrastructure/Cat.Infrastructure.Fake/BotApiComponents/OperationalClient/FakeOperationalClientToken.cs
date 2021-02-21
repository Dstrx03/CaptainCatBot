using System.Security.Cryptography;
using System.Text;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient
{
    public interface IFakeOperationalClientToken
    {
        bool IsTokenValid(string token);
    }

    public class FakeOperationalClientToken : IFakeOperationalClientToken
    {
        private readonly string _fakeTokenHash;

        public FakeOperationalClientToken(string token)
        {
            _fakeTokenHash = CalculateTokenHash(token);
        }

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            return _fakeTokenHash == CalculateTokenHash(token);
        }

        private string CalculateTokenHash(string token)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(token.Trim()));
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
