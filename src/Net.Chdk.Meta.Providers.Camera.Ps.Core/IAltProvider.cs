using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public interface IAltProvider
    {
        AltData GetAlt(string platform, string[]? altNames, string productName);
    }
}
