using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public interface ISoftwareProvider
    {
        SoftwareInfo GetSoftware(IMatchData? data, SoftwareInfo software);
    }
}
