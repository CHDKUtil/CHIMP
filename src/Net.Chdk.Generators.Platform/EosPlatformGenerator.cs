using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Generators.Platform
{
    sealed class EosPlatformGenerator : InnerPlatformGenerator
    {
        private const uint MinVideoModelId = 0x40000000;
        private const uint MinEosModelId = 0x80000000;

        private static readonly string[] PsEosModels = { "M3", "M5", "M6", "M10", "M100" };

        protected override string GetCategoryName(uint modelId, IEnumerable<string[]> splits)
        {
            if (modelId >= MinVideoModelId && modelId < MinEosModelId)
                return string.Empty;

            var split = splits.First();
            return split.Length == 2 && PsEosModels.Contains(split[1])
                ? "PS"
                : "EOS";
        }

        protected override string[]? PreGenerate(uint modelId, IEnumerable<string[]> splits)
        {
            var split = splits.First();
            if (split[0] != Keyword)
                return null;

            if (split[1] == "D30")
                return null;

            if (split.Contains("Rebel") || split[1].StartsWith("SL"))
                return splits.Skip(1).First();

            return split;
        }

        protected override IEnumerable<string> Process(string[] split)
        {
            return split.Length > 1
                ? AdaptMark(split.Skip(1).ToArray())
                : split;
        }

        protected override string Keyword => "EOS";
    }
}
