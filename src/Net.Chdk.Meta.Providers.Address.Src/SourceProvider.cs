using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model;
using Net.Chdk.Meta.Providers.Src;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class SourceProvider : SourceProvider<PlatformSourceData>
    {
        public SourceProvider(ILogger<SourceProvider> logger)
            : base(logger)
        {
        }
    }
}
