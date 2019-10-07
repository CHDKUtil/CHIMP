using System.Collections.Generic;

namespace Net.Chdk.Generators.Script
{
    public static class SubstitutesExtensions
    {
        public static bool TryGetValue<T>(this IDictionary<string, object> subs, string key, out T? result)
            where T : class
        {
            if (!subs.TryGetValue(key, out object value))
            {
                result = null;
                return false;
            }
            result = value as T;
            return result != null;
        }
    }
}
