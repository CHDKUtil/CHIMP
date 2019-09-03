using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public interface IAltProvider
    {
        AltData GetAlt(string platform, TreeAltData tree, string productName);
    }
}
