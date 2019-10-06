using Chimp.Model;
using Net.Chdk.Model.Software;

namespace Chimp.Providers.Supported
{
    abstract class SupportedProviderBase : ISupportedProvider
    {
        public string GetError(MatchData data)
        {
            return IsMatch(data)
                ? DoGetError(data)
                : null;
        }

        public string[] GetItems(MatchData data, SoftwareProductInfo product, SoftwareCameraInfo camera)
        {
            return IsMatch(data)
                ? DoGetItems(data, product, camera)
                : null;
        }

        public string GetTitle(MatchData data)
        {
            return IsMatch(data)
                ? DoGetTitle(data)
                : null;
        }

        protected abstract bool IsMatch(MatchData data);
        protected abstract string DoGetError(MatchData data);
        protected abstract string[] DoGetItems(MatchData data, SoftwareProductInfo product, SoftwareCameraInfo camera);
        protected abstract string DoGetTitle(MatchData data);
    }
}
