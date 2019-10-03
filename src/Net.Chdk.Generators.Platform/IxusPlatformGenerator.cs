using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Generators.Platform
{
    sealed class IxusPlatformGenerator : InnerPlatformGenerator
    {
        protected override string[]? PreGenerate(uint modelId, IEnumerable<string[]> splits)
        {
            foreach (var split in splits)
                if (split.Contains(Keyword))
                    return split;

            return null;
        }

        protected override IEnumerable<string> Process(string[] split)
        {
            var index = Array.IndexOf(split, Keyword);
            var split2 = split.Skip(index);

            if (index < split.Length - 2 && split[index + 1] != "i")
                split2 = split2.Take(2);

            return split2;
        }

        protected override string Keyword => "IXUS";
    }
}
