using System.Security.Cryptography;

namespace DemoAzAI.Application.Extensions
{

    public static class ByteArrayExtensions
    {
        public static string ComputeHash(this byte[] bytes)
        {
            return Convert.ToHexString(
                SHA256.HashData(bytes));
        }
    }
}
