using Net.Chdk.Model.Software;

namespace Net.Chdk.Meta.Providers.Software
{
    public interface ISourceMetaProvider
    {
        SoftwareSourceInfo GetSource(SoftwareInfo software);
    }
}
