using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Eos
{
    sealed class EosPsPlatformGenerator : InnerPlatformGenerator
    {
        private const uint MaxModelId = 0x1000000;

        protected override IEnumerable<string>? PreGenerate(uint modelId, string source)
        {
            if (modelId > MaxModelId)
                return null;

            var split = source.Split(' ');
            if (!Keyword.Equals(split[0]))
                return null;

            split = AdaptMark(split);

            return split.Skip(1);
        }

        protected override string Keyword => "PowerShot";

        protected override string[] Suffixes => new[] { "HS" };
    }
}
