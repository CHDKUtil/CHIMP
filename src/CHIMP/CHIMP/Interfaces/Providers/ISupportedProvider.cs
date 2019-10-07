using Chimp.Model;
using Net.Chdk.Model.Software;

namespace Chimp
{
    interface ISupportedProvider
    {
        string GetError(MatchData data);
        string[] GetItems(MatchData data, SoftwareProductInfo product, SoftwareCameraInfo camera);
        string GetTitle(MatchData data);
    }

    interface IInnerSupportedProvider : ISupportedProvider
    {
        bool IsMatch(MatchData data);
    }
}
