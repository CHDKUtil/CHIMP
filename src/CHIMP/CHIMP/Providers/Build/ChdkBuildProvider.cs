using Net.Chdk.Model.Software;
using System;

namespace Chimp.Providers.Build
{
    sealed class ChdkBuildProvider : BuildProvider
    {
        private SoftwareSourceInfo Source { get; }

        public ChdkBuildProvider(SoftwareSourceInfo source)
        {
            Source = source;
        }

        public override string GetBuildName(SoftwareInfo software)
        {
            var product = software?.Product;
            var source = software?.Source;
            if (product == null || source == null)
                return "full";
            return product.Version?.MinorRevision >= 0 && source.Name.Equals(Source.Name, StringComparison.InvariantCulture)
                ? string.Empty
                : "full";
        }
    }
}
