using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software.Chdk
{
    public sealed class ChdkBuildProvider : IBuildProvider
    {
        private SoftwareSourceInfo Source { get; }

        public ChdkBuildProvider(SoftwareSourceInfo source)
        {
            Source = source;
        }

        public string GetBuildName(SoftwareInfo software)
        {
            var product = software?.Product;
            var source = software?.Source;
            if (product == null || source == null)
                return "full";
            return product.Version?.MinorRevision >= 0 && source.Name == Source.Name
                ? string.Empty
                : "full";
        }
    }
}
