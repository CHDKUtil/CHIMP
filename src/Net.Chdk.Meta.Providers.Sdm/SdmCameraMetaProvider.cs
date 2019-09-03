using System.IO;

namespace Net.Chdk.Meta.Providers.Sdm
{
    sealed class SdmCameraMetaProvider : IProductCameraMetaProvider
    {
        public CameraInfo GetCamera(string name)
        {
            var split = Path.GetFileNameWithoutExtension(name).Split('-');
            return new CameraInfo
            {
                Platform = split[2],
                Revision = split[3]
            };
        }

        public string ProductName => "SDM";
    }
}
