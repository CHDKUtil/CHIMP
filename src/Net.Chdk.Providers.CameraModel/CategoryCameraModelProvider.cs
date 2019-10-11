using System;
using System.Globalization;
using System.Linq;
using Net.Chdk.Adapters.Platform;
using Net.Chdk.Meta.Model.Platform;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;

namespace Net.Chdk.Providers.CameraModel
{
    abstract class CategoryCameraModelProvider : ICategoryCameraModelProvider
    {
        private IPlatformAdapter PlatformAdapter { get; }
        private IPlatformProvider PlatformProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public CategoryCameraModelProvider(IPlatformAdapter platformAdapter, IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider)
        {
            PlatformAdapter = platformAdapter;
            PlatformProvider = platformProvider;
            FirmwareProvider = firmwareProvider;
        }

        public (SoftwareCameraInfo, SoftwareModelInfo)? GetCameraModel(CameraInfo camera, CameraModelInfo cameraModel)
        {
            var cameraInfo = GetCamera(camera, cameraModel);
            if (cameraInfo == null)
                return null;

            var modelInfo = GetModel(camera, cameraModel);
            if (modelInfo == null)
                return null;

            return (cameraInfo, modelInfo);
        }

        public (CameraInfo, CameraModelInfo[])? GetCameraModels(SoftwareCameraInfo? cameraInfo, SoftwareModelInfo? cameraModel)
        {
            if (cameraInfo is null || cameraModel is null || cameraModel.Name == null)
                return null;

            var camera = GetCamera(cameraModel.Id, cameraInfo.Revision, cameraModel.Name);
            if (camera == null)
                return null;

            return (camera, GetCameraModels(cameraModel.Name));
        }

        public (CameraInfo, CameraModelInfo[])? GetCameraModels(SoftwareProductInfo? product, SoftwareCameraInfo? cameraInfo)
        {
            if (product?.Name == null || cameraInfo?.Platform == null)
                return null;

            var key = PlatformAdapter.NormalizePlatform(product.Name, cameraInfo.Platform);
            var platform = GetPlatform(key);
            if (platform == null)
                return null;

            var camera = GetCamera(platform, cameraInfo.Revision);
            return (camera, GetCameraModels(platform));
        }

        public (CameraInfo, CameraModelInfo[])? GetCameraModels(CameraInfo camera)
        {
            var platforms = GetPlatforms(camera);
            if (platforms == null)
                return null;

            return (camera, GetCameraModels(platforms));
        }

        protected abstract string CategoryName { get; }

        private static CameraModelInfo[] GetCameraModels(params PlatformData[] platforms)
        {
            return platforms
                .Select(GetCameraModel)
                .ToArray();
        }

        private static CameraModelInfo GetCameraModel(PlatformData platform)
        {
            return GetCameraModel(platform.Names);
        }

        private static CameraModelInfo[] GetCameraModels(params string[]? names)
        {
            return new[]
            {
                GetCameraModel(names)
            };
        }

        private static CameraModelInfo GetCameraModel(string[]? names)
        {
            return new CameraModelInfo
            {
                Names = names
            };
        }

        private SoftwareCameraInfo? GetCamera(CameraInfo camera, CameraModelInfo cameraModel)
        {
            var platform = GetPlatform(camera, cameraModel);
            if (platform == null)
                return null;

            var revision = GetRevision(camera);
            if (revision == null)
                return null;

            return new SoftwareCameraInfo
            {
                Platform = platform,
                Revision = revision,
            };
        }

        private static SoftwareModelInfo? GetModel(CameraInfo camera, CameraModelInfo cameraModel)
        {
            if (camera.Canon.ModelId == 0)
                return null;

            return new SoftwareModelInfo
            {
                Id = camera.Canon.ModelId,
                Name = GetName(camera, cameraModel)
            };
        }

        private string? GetPlatform(CameraInfo camera, CameraModelInfo cameraModel)
        {
            return PlatformProvider.GetPlatform(camera, cameraModel, CategoryName);
        }

        private string? GetRevision(CameraInfo camera)
        {
            return FirmwareProvider.GetFirmwareRevision(camera, CategoryName);
        }

        // For N / N Facebook
        private static string GetName(CameraInfo camera, CameraModelInfo cameraModel)
        {
            return cameraModel.Names.Length == 1
                ? cameraModel.Names[0]
                : camera.Base.Model;
        }

        private PlatformData? GetPlatform(string platform)
        {
            return PlatformProvider.GetPlatform(platform, CategoryName);
        }

        private PlatformData[]? GetPlatforms(CameraInfo camera)
        {
            return PlatformProvider.GetPlatforms(camera, CategoryName);
        }

        private CameraInfo GetCamera(PlatformData platform, string? revision)
        {
            return new CameraInfo
            {
                Base = CreateBaseInfo(platform),
                Canon = CreateCanonInfo(platform, revision)
            };
        }

        private CameraInfo? GetCamera(uint modelId, string? revision, string? model)
        {
            var canon = CreateCanonInfo(modelId, revision);
            if (canon == null)
                return null;

            return new CameraInfo
            {
                Base = CreateBaseInfo(model),
                Canon = canon
            };
        }

        private static BaseInfo? CreateBaseInfo(PlatformData platform)
        {
            return new BaseInfo
            {
                Make = "Canon",
                Model = string.Join("\n", platform.Names)
            };
        }

        private static BaseInfo? CreateBaseInfo(string? model)
        {
            return new BaseInfo
            {
                Make = "Canon",
                Model = model
            };
        }

        private CanonInfo? CreateCanonInfo(PlatformData platform, string? revision)
        {
            if (platform?.ModelId == null || revision == null)
                return null;

            return new CanonInfo
            {
                ModelId = uint.Parse(platform.ModelId.Substring(2), NumberStyles.HexNumber),
                FirmwareRevision = GetFirmwareRevision(revision),
                FirmwareVersion = GetFirmwareVersion(revision)
            };
        }

        private CanonInfo? CreateCanonInfo(uint? modelId, string? revision)
        {
            if (modelId == null || revision == null)
                return null;

            var firmwareRevision = GetFirmwareRevision(revision);
            var firmwareVersion = GetFirmwareVersion(revision);
            if (firmwareRevision == 0 && firmwareVersion == null)
                return null;

            return new CanonInfo
            {
                ModelId = modelId.Value,
                FirmwareRevision = firmwareRevision,
                FirmwareVersion = firmwareVersion
            };
        }

        protected abstract uint GetFirmwareRevision(string revision);
        protected abstract Version? GetFirmwareVersion(string revision);
    }
}
