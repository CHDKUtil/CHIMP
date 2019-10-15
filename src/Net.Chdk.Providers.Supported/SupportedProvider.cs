using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Supported
{
    sealed class SupportedProvider : ISupportedProvider
    {
        private IEnumerable<IInnerSupportedProvider> Providers { get; }

        public SupportedProvider(IEnumerable<IInnerSupportedProvider> providers)
        {
            Providers = providers;
        }

        public string? GetError(IMatchData data)
        {
            return GetProvider(data)?
                .GetError(data);
        }

        public string[]? GetItems(IMatchData data, SoftwareInfo software)
        {
            return GetProvider(data)?
                .GetItems(data, software);
        }

        public string? GetTitle(IMatchData data)
        {
            return GetProvider(data)?
                .GetTitle(data);
        }

        private IInnerSupportedProvider? GetProvider(IMatchData data)
        {
            return Providers
                .FirstOrDefault(p => p.IsMatch(data));
        }
    }
}
