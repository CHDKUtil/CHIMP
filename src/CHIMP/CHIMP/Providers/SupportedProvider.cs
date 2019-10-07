using Chimp.Model;
using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers
{
    sealed class SupportedProvider : ISupportedProvider
    {
        private IEnumerable<IInnerSupportedProvider> Providers { get; }

        public SupportedProvider(IEnumerable<IInnerSupportedProvider> providers)
        {
            Providers = providers;
        }

        public string GetError(MatchData data)
        {
            return GetProvider(data)?
                .GetError(data);
        }

        public string[] GetItems(MatchData data, SoftwareProductInfo product, SoftwareCameraInfo camera)
        {
            return GetProvider(data)?
                .GetItems(data, product, camera);
        }

        public string GetTitle(MatchData data)
        {
            return GetProvider(data)?
                .GetTitle(data);
        }

        private IInnerSupportedProvider GetProvider(MatchData data)
        {
            return Providers
                .FirstOrDefault(p => p.IsMatch(data));
        }
    }
}
