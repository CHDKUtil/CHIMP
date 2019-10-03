using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Generators.Platform
{
    sealed class PsPlatformGenerator : InnerPlatformGenerator
    {
        private const uint MinModelId = 0x1000000;

        private static readonly string[] Exceptions = new[] { "X", "Facebook" };

        protected override string? GetCategoryName(uint modelId, IEnumerable<string[]> splits)
        {
            return modelId < MinModelId
                ? "EOS"
                : base.GetCategoryName(modelId, splits);
        }

        protected override string[]? PreGenerate(uint modelId, IEnumerable<string[]> splits)
        {
            var split = splits.First();
            if (split[0] != Keyword)
                return null;

            return split;
        }

        protected override IEnumerable<string> Process(string[] split)
        {
            return split.Length > 2 && !Exceptions.Contains(split[2])
                ? new[] { split[1] }
                : AdaptMark(split.Skip(1).ToArray());
        }

        protected override string Keyword => "PowerShot";
    }
}
