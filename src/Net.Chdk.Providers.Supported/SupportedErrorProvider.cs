using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;

namespace Net.Chdk.Providers.Supported
{
    sealed class SupportedErrorProvider : IInnerSupportedProvider
    {
        public bool IsMatch(IMatchData data)
        {
            return data.Error != null;
        }

        public string? GetError(IMatchData data)
        {
            return data.Error;
        }

        public string[]? GetItems(IMatchData data, SoftwareInfo software)
        {
            return null;
        }

        public string? GetTitle(IMatchData data)
        {
            return null;
        }
    }
}
