using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkCameraBootProvider : PsCameraBootProvider
    {
        public override string ProductName => "CHDK";
    }
}
