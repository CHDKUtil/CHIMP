using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public interface ISoftwareData
    {
        IMatchData Match { get; }
        SoftwareInfo Info { get; }
    }
}
