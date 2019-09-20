using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    abstract class PsPlatformGeneratorBase : InnerPlatformGenerator
    {
        private const int MinModelId = 0x1540000;

        protected override IEnumerable<string> PreGenerate(uint modelId, string source)
        {
            if (modelId < MinModelId)
                return null;

            var split = source.Split(' ');
            if (!split.Contains(Keyword))
                return null;

            return split.Skip(1);
        }
    }
}
