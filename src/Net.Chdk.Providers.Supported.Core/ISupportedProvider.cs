using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;

namespace Net.Chdk.Providers.Supported
{
    public interface ISupportedProvider
    {
        string? GetError(IMatchData data);
        string[]? GetItems(IMatchData data, SoftwareInfo software);
        string? GetTitle(IMatchData data);
    }
}
