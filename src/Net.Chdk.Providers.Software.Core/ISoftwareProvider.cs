using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public interface ISoftwareProvider
    {
    }

    public interface ISoftwareProvider<TMatchData> : ISoftwareProvider
        where TMatchData : class, IMatchData
    {
        SoftwareInfo GetSoftware(TMatchData? data, SoftwareInfo software);
    }
}
