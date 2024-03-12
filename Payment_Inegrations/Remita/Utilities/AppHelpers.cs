using System.Collections.Generic;

namespace Remita.Utilities
{
    public static class AppHelpers
    {
        public static bool IsNullOrEmpty<T>(this IList<T> source)
        {
            if (source == null)
                return true;

            return source.Count == 0;
        }
    }
}
