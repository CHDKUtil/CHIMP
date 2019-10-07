using Chimp.Model;
using Net.Chdk.Model.Software;
using System;

namespace Chimp.Providers.Supported
{
    sealed class SupportedErrorProvider : IInnerSupportedProvider
    {
        public bool IsMatch(MatchData data)
        {
            return data.Error != null;
        }

        public string GetError(MatchData data)
        {
            return data.Error;
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
