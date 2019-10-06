using Chimp.Model;
using Chimp.Properties;
using Net.Chdk.Model.Software;
using System;

namespace Chimp.Providers.Supported
{
    sealed class SupportedBuildProvider : SupportedProviderBase
    {
        protected override bool IsMatch(MatchData data)
        {
            return data.Builds != null;
        }

        protected override string DoGetError(MatchData data)
        {
            return Resources.Download_InvalidFormat_Text;
        }

        protected override string[] DoGetItems(MatchData data, SoftwareProductInfo product, SoftwareCameraInfo camera)
        {
            return Array.Empty<string>();
        }

        protected override string DoGetTitle(MatchData data)
        {
            return string.Empty;
        }
    }
}
