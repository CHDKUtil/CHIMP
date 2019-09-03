using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    sealed class IxusPlatformGenerator : InnerPlatformGenerator, IIxusPlatformGenerator
    {
        protected override IEnumerable<string> PreGenerate(string source)
        {
            var split = source.Split(' ');
            var index = Array.IndexOf(split, Keyword); //Skip Digital
            if (index < 0)
                return null;

            return split.Skip(index);
        }

        protected override IEnumerable<string> Process(IEnumerable<string> split)
        {
            return split
                .Select(s => s == "Wireless" ? "w" : s);
        }

        protected override string Keyword => "IXUS";

        protected override string[] Suffixes => new[] { "IS", "HS", "Ti" };
    }
}
