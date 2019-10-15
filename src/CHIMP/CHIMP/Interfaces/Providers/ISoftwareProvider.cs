using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;

namespace Chimp
{
    interface ISoftwareProvider
    {
        SoftwareInfo GetSoftware(IMatchData data, SoftwareInfo software);
    }
}
