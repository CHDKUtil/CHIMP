using Microsoft.Extensions.Logging;
using System;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class HeaderParsingProvider<T> : ParsingProvider<T>
        where T : class
    {
        protected HeaderParsingProvider(ILogger logger)
            : base(logger)
        {
        }

        protected sealed override string TrimComments(string line, string platform, string? revision)
        {
            var index = line.IndexOf("//");
            if (index >= 0)
                line = line.Substring(0, index).TrimEnd();

            index = line.IndexOf("/*");
            if (index > 0)
            {
                var name = GetName(platform, revision);
                throw new InvalidOperationException($"{name}: Unexpected multi-line comment");
            }

            return line;
        }

        protected override string Prefix => "#define";
    }
}
