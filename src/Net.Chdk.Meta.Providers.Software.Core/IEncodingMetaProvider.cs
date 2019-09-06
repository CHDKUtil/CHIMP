using Net.Chdk.Model.Software;

namespace Net.Chdk.Meta.Providers.Software
{
    public interface IEncodingMetaProvider
    {
        SoftwareEncodingInfo GetEncoding(SoftwareInfo software);
    }
}
