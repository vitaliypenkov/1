using Common;

namespace BasicCoreApi.Helpers
{
    public static class CryptoHelper
    {
        internal static object Encrypt(string value)
        {
           return value.Encrypt(Constants.SECRET_KEY);
        }

        internal static string Decrypt(string value)
        {
            return value.FromBase64().Decrypt(Constants.SECRET_KEY);
        }
    }
}
