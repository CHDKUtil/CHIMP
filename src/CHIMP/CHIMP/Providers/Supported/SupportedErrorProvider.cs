using Chimp.Model;
using Net.Chdk.Model.Software;
using System;

namespace Chimp.Providers.Supported
{
    sealed class SupportedErrorProvider : SupportedProviderBase
    {
        protected override bool IsMatch(MatchData data)
        {
            return data.Error != null;
        }

        protected override string DoGetError(MatchData data)
        {
            return data.Error;
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
