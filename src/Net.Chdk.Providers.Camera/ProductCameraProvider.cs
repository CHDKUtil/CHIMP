using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers.Camera
{
    abstract class ProductCameraProvider<TCamera, TModel, TCard, TReverse, TRevision, TVersion> : DataProvider<Dictionary<string, TCamera>>, IProductCameraProvider
        where TCamera : CameraData<TCamera, TModel, TRevision, TCard>
        where TModel : CameraModelData<TModel, TRevision>
        where TCard : CardData
        where TRevision : IRevisionData
        where TReverse : ReverseCameraData, new()
    {
        private const string DataFileName = "cameras.json";

        private string ProductName { get; }

        protected ProductCameraProvider(string productName, ILogger logger)
            : base(logger)
        {
            ProductName = productName;
            _reverseCameras = new Lazy<Dictionary<string, TReverse>>(GetReverseCameras);
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
                Revision = GetRevision(cameraInfo, model),
            };
        }

        protected abstract string GetRevision(CameraInfo cameraInfo, TModel model);

        public SoftwareEncodingInfo GetEncoding(SoftwareCameraInfo cameraInfo)
        {
            return GetCameraModel(cameraInfo, out TReverse camera)
                ? camera.Encoding
                : null;
        }

        public AltInfo GetAlt(SoftwareCameraInfo cameraInfo)
        {
            return GetCameraModel(cameraInfo, out TReverse reverse)
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

        protected virtual CameraModelsInfo GetCameraModels(TCamera camera, CameraModelInfo[] models)
        {
            return new CameraModelsInfo
            {
                Models = models,
                CardType = camera.Card?.Type,
                CardSubtype = camera.Card?.Subtype,
                BootFileSystem = camera.Boot?.Fs,
            };
        }

        public CameraModelsInfo GetCameraModels(SoftwareCameraInfo camera)
        {
            if (!GetCameraModel(camera, out TReverse reverse))
                return null;

            if (!GetCamera(reverse, camera, out TVersion version))
                return null;

            return GetCameraModels(reverse, version);
        }

        protected virtual CameraModelsInfo GetCameraModels(TReverse camera, TVersion version)
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

        protected abstract bool IsInvalid(CameraInfo cameraInfo);

        protected abstract CanonInfo CreateCanonInfo(TReverse camera, TVersion version);

        private static BaseInfo CreateBaseInfo(TReverse camera)
        {
            return new BaseInfo
            {
                Make = "Canon",
                Model = string.Join("\n", camera.Models)
            };
        }

        protected abstract bool GetCamera(TReverse reverse, SoftwareCameraInfo camera, out TVersion version);

        private TCamera GetCamera(CameraInfo cameraInfo)
        {
            if (IsInvalid(cameraInfo))
                return null;

            var modelId = $"0x{cameraInfo.Canon.ModelId:x}";
            if (!Data.TryGetValue(modelId, out TCamera camera))
                return null;

            return camera;
        }

        protected bool GetCameraModel(SoftwareCameraInfo camera, out TReverse reverse)
        {
            reverse = null;

            if (camera == null)
                return false;

            return ReverseCameras.TryGetValue(camera.Platform, out reverse);
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Product, ProductName, DataFileName);
        }

        #region ReverseCameras

        private readonly Lazy<Dictionary<string, TReverse>> _reverseCameras;

        private Dictionary<string, TReverse> ReverseCameras => _reverseCameras.Value;

        private Dictionary<string, TReverse> GetReverseCameras()
        {
            var reverseCameras = new Dictionary<string, TReverse>();
            foreach (var kvp in Data)
            {
                foreach (var model in kvp.Value.Models)
                {
                    var camera = CreateReverseCamera(kvp.Key, kvp.Value, model);
                    reverseCameras.Add(model.Platform, camera);
                }
            }
            return reverseCameras;
        }

        protected virtual TReverse CreateReverseCamera(string key, TCamera camera, TModel model)
        {
            return new TReverse
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
