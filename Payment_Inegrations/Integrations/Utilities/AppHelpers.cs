using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Integrations.Utilities
{
    public static class AppHelpers
    {
        public static bool IsNullOrEmpty<T>(this IList<T> source)
        {
            if (source == null)
                return true;

            return source.Count == 0;
        }

        public static string ComputeSha512Hash(string rawData)
        {
            using(SHA512 sHA512 = SHA512.Create())
            {
                byte[] bytes = sHA512.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    stringBuilder.Append(bytes[i].ToString("x2"));

                return stringBuilder.ToString();
            }
        }
    }
}
