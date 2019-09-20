using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    sealed class IxusPlatformGenerator : PsPlatformGeneratorBase, IIxusPlatformGenerator
    {
        protected override IEnumerable<string> PreGenerate(uint modelId, string source)
        {
            var split = base.PreGenerate(modelId, source);
            if (split == null)
                return null;

            if (!Keyword.Equals(split.First())) // No "Digital" prefix
                split = new[] { Keyword }.Concat(split);

            return split;
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
