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

namespace Net.Chdk.Providers.Camera
{
    abstract class CategoryCameraProvider : ICategoryCameraProvider
    {
        private IPlatformAdapter PlatformAdapter { get; }
        private IPlatformProvider PlatformProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public CategoryCameraProvider(IPlatformAdapter platformAdapter, IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider)
        {
            PlatformAdapter = platformAdapter;
            PlatformProvider = platformProvider;
            FirmwareProvider = firmwareProvider;
        }

        public SoftwareCameraInfo GetCamera(CameraInfo camera, CameraModelInfo cameraModel)
        {
            return new SoftwareCameraInfo
            {
                Platform = GetPlatform(camera, cameraModel),
                Revision = GetRevision(camera),
            };
        }

        public CameraModelsInfo? GetCameraModels(CameraInfo camera)
        {
            var platforms = GetPlatforms(camera);
            if (platforms == null)
                return null;

            return GetCameraModels(camera, platforms);
        }

        public CameraModelsInfo? GetCameraModels(SoftwareProductInfo? product, SoftwareCameraInfo? cameraInfo)
        {
            if (product == null || cameraInfo == null)
                return null;

            var key = PlatformAdapter.NormalizePlatform(product.Name, cameraInfo.Platform);
            var platform = GetPlatform(key);
            if (platform == null)
                return null;

            var camera = GetCamera(platform, cameraInfo.Revision);
            return GetCameraModels(camera, platform);
        }

        protected abstract string CategoryName { get; }

        private static CameraModelsInfo GetCameraModels(CameraInfo camera, params PlatformData[] platforms)
        {
            return new CameraModelsInfo
            {
                Info = camera,
                Models = GetCameraModels(platforms),
            };
        }

        private static CameraModelInfo[] GetCameraModels(PlatformData[] platforms)
        {
            return platforms
                .Select(GetCameraModel)
                .ToArray();
        }

        private static CameraModelInfo GetCameraModel(PlatformData platform)
        {
            return new CameraModelInfo
            {
                Names = platform.Names
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

        private PlatformData? GetPlatform(string platform)
        {
            return PlatformProvider.GetPlatform(platform, CategoryName);
        }

        private PlatformData[]? GetPlatforms(CameraInfo camera)
        {
            return PlatformProvider.GetPlatforms(camera, CategoryName);
        }

        private CameraInfo GetCamera(PlatformData platform, string revision)
        {
            return new CameraInfo
            {
                Base = CreateBaseInfo(platform),
                Canon = CreateCanonInfo(platform, revision)
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

        private CanonInfo? CreateCanonInfo(PlatformData platform, string revision)
        {
            if (platform?.ModelId == null)
                return null;

            return new CanonInfo
            {
                ModelId = uint.Parse(platform.ModelId.Substring(2), NumberStyles.HexNumber),
                FirmwareRevision = GetFirmwareRevision(revision),
                FirmwareVersion = GetFirmwareVersion(revision),
            };
        }

        protected abstract uint GetFirmwareRevision(string revision);
        protected abstract Version? GetFirmwareVersion(string revision);
    }
}
