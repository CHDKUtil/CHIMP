using Chimp.Model;
using Chimp.Properties;
using Net.Chdk.Model.Software;
using System;

namespace Chimp.Providers.Supported
{
    sealed class SupportedBuildProvider : IInnerSupportedProvider
    {
        public bool IsMatch(MatchData data)
        {
            return data.Builds != null;
        }

        public string GetError(MatchData data)
        {
            return Resources.Download_InvalidFormat_Text;
        }

        public string[] GetItems(MatchData data, SoftwareProductInfo product, SoftwareCameraInfo camera)
        {
            return Array.Empty<string>();
        }

        public string GetTitle(MatchData data)
        {
            return string.Empty;
        }
    }
}
