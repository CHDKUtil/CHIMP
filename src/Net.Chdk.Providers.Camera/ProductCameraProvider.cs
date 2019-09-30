using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers.Camera
{
    abstract class ProductCameraProvider<TCamera, TCard> : DataProvider<Dictionary<string, TCamera>>, IProductCameraProvider
        where TCamera : CameraData<TCamera, TCard>
        where TCard : CardData
    {
        private const string DataFileName = "cameras.json";

        private string ProductName { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        protected ProductCameraProvider(string productName, IFirmwareProvider firmwareProvider, ILogger logger)
            : base(logger)
        {
            ProductName = productName;
            FirmwareProvider = firmwareProvider;

            _reverseCameras = new Lazy<Dictionary<string, ReverseCameraData>>(GetReverseCameras);
        }

        public SoftwareCameraInfo GetCamera(CameraInfo cameraInfo, CameraModelInfo cameraModelInfo)
        {
            var camera = GetCamera(cameraInfo);
            if (camera == null)
                return null;

            var model = camera.Models.SingleOrDefault(m => m.Names[0].Equals(cameraModelInfo.Names[0], StringComparison.Ordinal));
            if (model == null)
                return null;

            return new SoftwareCameraInfo
            {
                Platform = model.Platform,
                Revision = GetRevision(cameraInfo),
            };
        }

        public SoftwareEncodingInfo GetEncoding(SoftwareCameraInfo cameraInfo)
        {
            return GetCameraModel(cameraInfo, out ReverseCameraData camera)
                ? camera.Encoding
                : null;
        }

        public AltInfo GetAlt(SoftwareCameraInfo cameraInfo)
        {
            return GetCameraModel(cameraInfo, out ReverseCameraData reverse)
                ? reverse.Alt
                : null;
        }

        public CameraModelsInfo GetCameraModels(CameraInfo cameraInfo)
        {
            var camera = GetCamera(cameraInfo);
            if (camera == null)
                return null;

            var models = new CameraModelInfo[camera.Models.Length];
            for (var i = 0; i < camera.Models.Length; i++)
            {
                models[i] = new CameraModelInfo
                {
                    Names = camera.Models[i].Names
                };
            }
            return GetCameraModels(camera, models);
        }

        private CameraModelsInfo GetCameraModels(TCamera camera, CameraModelInfo[] models)
        {
            return new CameraModelsInfo
            {
                Models = models,
                CardType = camera.Card?.Type,
                CardSubtype = camera.Card?.Subtype,
                BootFileSystem = camera.Boot?.Fs,
                IsMultiPartition = IsMultiPartition(camera),
            };
        }

        public CameraModelsInfo GetCameraModels(SoftwareCameraInfo camera)
        {
            if (!GetCameraModel(camera, out ReverseCameraData reverse))
                return null;

            return GetCameraModels(reverse, camera.Revision);
        }

        private string GetRevision(CameraInfo cameraInfo)
        {
            return FirmwareProvider.GetFirmwareRevision(cameraInfo);
        }

        private CameraModelsInfo GetCameraModels(ReverseCameraData camera, string version)
        {
            return new CameraModelsInfo
            {
                Info = new CameraInfo
                {
                    Base = CreateBaseInfo(camera),
                    Canon = CreateCanonInfo(camera, version),
                },
                Models = new[]
                {
                    new CameraModelInfo
                    {
                        Names = camera.Models
                    }
                },
            };
        }

        private static BaseInfo CreateBaseInfo(ReverseCameraData camera)
        {
            return new BaseInfo
            {
                Make = "Canon",
                Model = string.Join("\n", camera.Models)
            };
        }

        private CanonInfo CreateCanonInfo(ReverseCameraData camera, string revision)
        {
            return new CanonInfo
            {
                ModelId = camera.ModelId,
                FirmwareRevision = GetFirmwareRevision(revision),
                FirmwareVersion = GetFirmwareVersion(revision),
            };
        }

        private TCamera GetCamera(CameraInfo cameraInfo)
        {
            if (IsInvalid(cameraInfo))
                return null;

            var modelId = $"0x{cameraInfo.Canon.ModelId:x}";
            if (!Data.TryGetValue(modelId, out TCamera camera))
                return null;

            return camera;
        }

        protected bool GetCameraModel(SoftwareCameraInfo camera, out ReverseCameraData reverse)
        {
            reverse = null;

            if (camera?.Platform == null)
                return false;

            return ReverseCameras.TryGetValue(camera.Platform, out reverse);
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Product, ProductName, DataFileName);
        }

        protected abstract bool IsInvalid(CameraInfo cameraInfo);

        protected abstract bool IsMultiPartition(TCamera camera);

        protected abstract uint GetFirmwareRevision(string revision);

        protected abstract Version GetFirmwareVersion(string revision);

        #region ReverseCameras

        private readonly Lazy<Dictionary<string, ReverseCameraData>> _reverseCameras;

        private Dictionary<string, ReverseCameraData> ReverseCameras => _reverseCameras.Value;

        private Dictionary<string, ReverseCameraData> GetReverseCameras()
        {
            var reverseCameras = new Dictionary<string, ReverseCameraData>();
            foreach (var kvp in Data)
            {
                foreach (var model in kvp.Value.Models)
                {
                    var camera = GetReverseCamera(kvp.Key, kvp.Value, model);
                    reverseCameras.Add(model.Platform, camera);
                }
            }
            return reverseCameras;
        }

        private ReverseCameraData GetReverseCamera(string key, TCamera camera, CameraModelData model)
        {
            return new ReverseCameraData
            {
                ModelId = Convert.ToUInt32(key, 16),
                Encoding = GetEncoding(camera),
                Alt = GetAlt(camera),
                Models = model.Names,
            };
        }

        protected abstract SoftwareEncodingInfo GetEncoding(TCamera camera);

        protected abstract AltInfo GetAlt(TCamera camera);

        #endregion
    }
}
