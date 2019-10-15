using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System;

namespace Net.Chdk.Providers.Supported
{
    sealed class SupportedBuildProvider : IInnerSupportedProvider
    {
        public bool IsMatch(IMatchData data)
        {
            return data.Builds != null;
        }

        public string GetError(IMatchData data)
        {
            return "Download_InvalidFormat_Text";
        }

        public string[] GetItems(IMatchData data, SoftwareInfo software)
        {
            return Array.Empty<string>();
        }

        public string GetTitle(IMatchData data)
        {
            return string.Empty;
        }
    }
}
