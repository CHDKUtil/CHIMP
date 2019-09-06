using System.Linq;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;

namespace Net.Chdk.Meta.Providers.Software
{
    sealed class SourceMetaProvider : ISourceMetaProvider
    {
        private ISourceProvider SourceProvider { get; }

        public SourceMetaProvider(ISourceProvider sourceProvider)
        {
            SourceProvider = sourceProvider;
        }

        public SoftwareSourceInfo GetSource(SoftwareInfo software)
        {
            return SourceProvider.GetSources(software.Product).Single().Source;
        }
    }
}
