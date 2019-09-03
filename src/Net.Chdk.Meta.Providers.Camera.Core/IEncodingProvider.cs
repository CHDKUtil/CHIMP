using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IEncodingProvider
    {
        EncodingData GetEncoding(string platform, uint version, string categoryName);
    }
}
