using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    sealed class PsEosPlatformGenerator : PsPlatformGeneratorBase
    {
        protected override IEnumerable<string> Process(IEnumerable<string> split)
        {
            if (split.Contains("Rebel"))
                return null;

            return split;
        }

        protected override string Keyword => "EOS";
    }
}
