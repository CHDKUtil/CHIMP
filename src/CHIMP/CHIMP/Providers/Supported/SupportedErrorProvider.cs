using Chimp.Model;
using Net.Chdk.Model.Software;

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

        public string[] GetItems(MatchData data)
        {
            return null;
        }

        public string GetTitle(MatchData data)
        {
            return null;
        }
    }
}
