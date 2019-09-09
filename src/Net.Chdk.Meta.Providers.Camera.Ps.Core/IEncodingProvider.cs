using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public interface IEncodingProvider
    {
        EncodingData GetEncoding(string platform, uint version);
    }
}
