using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    sealed class PsEosPlatformGenerator : InnerPlatformGenerator
    {
        protected override IEnumerable<string> Process(IEnumerable<string> split)
        {
            if (split.Contains("Rebel") != false)
                return null;

            return split;
        }

        protected override string Keyword => "EOS";
    }
}
