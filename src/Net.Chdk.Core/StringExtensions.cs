namespace Net.Chdk
{
    public static class StringExtensions
    {
        public static string TrimStart(this string str, string prefix)
        {
            return str.StartsWith(prefix)
                ? str.Substring(prefix.Length)
                : str;
        }

        public static string TrimEnd(this string str, string suffix)
        {
            return str.EndsWith(suffix)
                ? str.Substring(0, str.Length - suffix.Length)
                : str;
        }
    }
}
