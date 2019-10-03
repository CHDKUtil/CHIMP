using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.CameraModel
{
    abstract class ProductCameraProvider<TCamera, TCard> : DataProvider<Dictionary<string, TCamera>>, IProductCameraProvider
        where TCamera : CameraData<TCamera, TCard>
        where TCard : CardData
    {
        private const string DataFileName = "cameras.json";

        private string ProductName { get; }

        protected ProductCameraProvider(string productName, ILogger logger)
            : base(logger)
        {
            ProductName = productName;
        }

        public abstract SoftwareEncodingInfo? GetEncoding(SoftwareCameraInfo cameraInfo);

        public abstract string? GetAltButton(SoftwareCameraInfo cameraInfo);

        public string? GetCardType(CameraInfo cameraInfo)
        {
            var camera = GetCamera(cameraInfo);
            return camera?.Card?.Type;
        }

        public string? GetCardSubtype(CameraInfo cameraInfo)
        {
            var camera = GetCamera(cameraInfo);
            return camera?.Card?.Subtype;
        }

        public string? GetBootFileSystem(CameraInfo cameraInfo)
        {
            var camera = GetCamera(cameraInfo);
            return camera?.Boot?.Fs;
        }

        public bool IsMultiPartition(CameraInfo cameraInfo)
        {
            var camera = GetCamera(cameraInfo);
            return IsMultiPartition(camera);
        }

        private TCamera? GetCamera(CameraInfo cameraInfo)
        {
            if (IsInvalid(cameraInfo))
                return null;

            var modelId = $"0x{cameraInfo.Canon.ModelId:x}";
            if (!Data.TryGetValue(modelId, out TCamera camera))
                return null;

            return camera;
        }

        protected abstract bool IsMultiPartition(TCamera? camera);

        protected sealed override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Product, ProductName, DataFileName);
        }

        protected abstract bool IsInvalid(CameraInfo cameraInfo);
    }
}
