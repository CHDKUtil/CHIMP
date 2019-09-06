using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public interface ISoftwareHashProvider
    {
        SoftwareHashInfo GetHash(byte[] buffer, string fileName, string hashName);
    }
}
