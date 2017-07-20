using Net.Chdk.Detectors.CameraModel;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Exif;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ChdkUtility.Providers.Cameras
{
    sealed class ExifCameraProvider : IProductCameraProvider, IProductCameraModelDetector
    {
        private const uint MinFat32ModelId = 0x2980000;

        SoftwareCameraInfo IProductCameraProvider.GetCamera(string productName, CameraInfo cameraInfo, CameraModelInfo cameraModelInfo)
        {
            if (!new[] { "SDM" }.Contains(productName, StringComparer.InvariantCulture))
                return null;

            if (cameraInfo.Canon?.ModelId == null)
                return null;
            var modelId = $"0x{cameraInfo.Canon.ModelId:x}";
            if (!Cameras.TryGetValue(modelId, out Camera camera))
                return null;

            var model = camera.Models.SingleOrDefault(m => m.Names[0].Equals(cameraModelInfo.Names[0]));
            if (model == null)
                return null;

            //TODO
            var platform = model.Prefix;
            if (productName?.Equals("SDM") == true)
                platform = platform.Split('_')[0];

            return new SoftwareCameraInfo
            {
                Platform = platform,
                Revision = GetFirmwareRevision(cameraInfo)
            };
        }

        SoftwareEncodingInfo IProductCameraProvider.GetEncoding(SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo)
        {
            return null;
        }

        CameraModels IProductCameraModelDetector.GetCameraModels(SoftwareInfo softwareInfo, IProgress<double> progress, CancellationToken token)
        {
            if (!new[] { "SDM" }.Contains(softwareInfo?.Product?.Name, StringComparer.InvariantCulture))
                return null;

            if (softwareInfo?.Camera == null)
                return null;

            //TODO
            foreach (var kvp in Cameras)
            {
                //TODO
                foreach (var model in kvp.Value.Models)
                {
                    //TODO
                    if (model.Prefix.StartsWith(softwareInfo.Camera.Platform))
                    {
                        return GetCameraModels(model, kvp.Key, kvp.Value, softwareInfo.Camera);
                    }
                }
            }

            return null;
        }

        private CameraModels GetCameraModels(CameraModel model, string modelId, Camera camera, SoftwareCameraInfo cameraInfo)
        {
            return new CameraModels
            {
                Info = GetCameraInfo(model, cameraInfo, modelId),
                Models = new[] { GetCameraModelInfo(model) },
            };
        }

        private CameraInfo GetCameraInfo(CameraModel model, SoftwareCameraInfo cameraInfo, string modelId)
        {
            return new CameraInfo
            {
                Base = new BaseInfo
                {
                    Make = "Canon",
                    Model = string.Join("\n", model.Names)
                },
                Canon = new CanonInfo
                {
                    ModelId = Convert.ToUInt32(modelId, 16),
                    FirmwareRevision = GetFirmwareRevision(cameraInfo.Revision)
                },
            };
        }

        private static CameraModelInfo GetCameraModelInfo(CameraModel model)
        {
            return new CameraModelInfo
            {
                Names = model.Names,
            };
        }

        private static readonly Lazy<Dictionary<string, Camera>> cameras = new Lazy<Dictionary<string, Camera>>(GetCameras);

        private static Dictionary<string, Camera> Cameras => cameras.Value;

        private static Dictionary<string, Camera> GetCameras()
        {
            using (var reader = new StringReader(Properties.Resources.cameras))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = JsonSerializer.CreateDefault();
                return serializer.Deserialize<Dictionary<string, Camera>>(jsonReader);
            }
        }

        private static string GetFirmwareRevision(CameraInfo info)
        {
            uint revision = info.Canon.FirmwareRevision;
            return new string(new[] {
                (char)(((revision >> 24) & 0x0f) + 0x30),
                (char)(((revision >> 20) & 0x0f) + 0x30),
                (char)(((revision >> 16) & 0x0f) + 0x30),
                (char)(((revision >>  8) & 0x7f) + 0x60)
            });
        }

        private uint GetFirmwareRevision(string revision)
        {
            if (revision == null)
                return 0;
            return
                (uint)((revision[0] - 0x30) << 24) +
                (uint)((revision[1] - 0x30) << 20) +
                (uint)((revision[2] - 0x30) << 16) +
                (uint)((revision[3] - 0x60) << 8);
        }
    }
}
