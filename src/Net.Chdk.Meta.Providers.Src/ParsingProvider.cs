using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class ParsingProvider<T> : ValueProvider
        where T : class
    {
        protected ParsingProvider(ILogger logger)
            : base(logger)
        {
        }

        protected T? GetValue(string platformPath, string platform, string? revision)
        {
            T? value = default;

            var filePath = GetFilePath(platformPath, platform, revision);
            using (var reader = File.OpenText(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith(Prefix))
                    {
                        line = TrimComments(line, platform, revision);
                        line = line.Substring(Prefix.Length).TrimStart();
                        UpdateValue(ref value, line, platform);
                    }
                }
            }

            return value;
        }

        protected abstract string Prefix { get; }

        protected abstract void UpdateValue(ref T? value, string line, string platform);

        protected abstract string TrimComments(string line, string platform, string? revision);

        protected string[] ParseArray(string[] split, string platform)
        {
            var skip = split.SkipWhile(s => s.Length == 0 || s[0] != '{');
            var concat = string.Concat(skip);
            var trim = TrimBraces(concat, platform);
            var split2 = trim.Split(',');
            return TrimEnd(split2, platform);
        }

        private string[] TrimEnd(string[] split, string platform)
        {
            int index = split.Length - 1;
            while (index >= 0 && split[index].Length == 0)
                --index;

            if (index < split.Length - 1)
            {
                if (index == 0)
                {
                    var name = GetName(platform);
                    throw new InvalidOperationException($"{name}: Empty array");
                }
                Array.Resize(ref split, index + 1);
            }

            return split;
        }

        protected static string TrimParentheses(string str, string platform)
        {
            var first = str[0];
            if (first != '(')
                throw new InvalidOperationException($"{platform}: Unexpected character {first}");

            str = str.Substring(1).TrimStart();

            var last = str[str.Length - 1];
            if (last != ')')
                throw new InvalidOperationException($"{platform}: Unexpected character {last}");

            return str.Substring(0, str.Length - 1).TrimEnd();
        }

        protected static string TrimBraces(string str, string platform)
        {
            var first = str[0];
            if (first != '{')
                throw new InvalidOperationException($"{platform}: Unexpected character {first}");

            str = str.Substring(1).TrimStart();

            var last = str[str.Length - 1];
            if (last != '}')
                throw new InvalidOperationException($"{platform}: Unexpected character {last}");

            return str.Substring(0, str.Length - 1).TrimEnd();
        }

        protected static string TrimQuotes(string str, string platform)
        {
            var first = str[0];
            if (first != '"')
                throw new InvalidOperationException($"{platform}: Unexpected character {first}");

            str = str.Substring(1);

            var last = str[str.Length - 1];
            if (last != '"')
                throw new InvalidOperationException($"{platform}: Unexpected character {last}");

            return str.Substring(0, str.Length - 1);
        }
    }
}
