using Net.Chdk.Providers.Software;

namespace Net.Chdk.Providers.Supported
{
    interface IInnerSupportedProvider : ISupportedProvider
    {
        bool IsMatch(IMatchData data);
    }
}
