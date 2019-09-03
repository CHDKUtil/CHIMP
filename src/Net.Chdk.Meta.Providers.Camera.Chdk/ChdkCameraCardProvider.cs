using Net.Chdk.Meta.Providers.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkCameraCardProvider : PsCameraCardProvider
    {
        public override string ProductName => "CHDK";
    }
}
