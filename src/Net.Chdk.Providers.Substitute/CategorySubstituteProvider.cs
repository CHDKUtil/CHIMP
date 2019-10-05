using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Providers.Firmware;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Providers.Substitute
{
    abstract class CategorySubstituteProvider : DataProvider<Dictionary<string, AddressPlatformData>>, ICategorySubstituteProvider
    {
        private IPlatformGenerator PlatformGenerator { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public CategorySubstituteProvider(IPlatformGenerator platformGenerator, IFirmwareProvider firmwareProvider, ILogger logger)
            : base(logger)
        {
            PlatformGenerator = platformGenerator;
            FirmwareProvider = firmwareProvider;
        }

        public IDictionary<string, string>? GetSubstitutes(CameraInfo camera, CameraModelInfo cameraModel)
        {
            var platform = GetPlatform(camera, cameraModel);
            if (platform == null)
                return null;

            var revision = GetRevision(camera);
            if (revision == null)
                return null;

            if (!Data.TryGetValue(platform, out AddressPlatformData platformData))
                return null;

            var subs = new Dictionary<string, string>
            {
                ["model"] = cameraModel.Names[0],
                ["platform"] = platform,
                ["platform_id"] = GetHexString(platformData.Id),
            };

            if (platformData.Revisions != null && platformData.Revisions.TryGetValue(revision, out AddressRevisionData revisionData))
            {
                subs["revision"] = revision;
                subs["palette_buffer_ptr"] = GetHexString(revisionData.PaletteBufferPtr);
                subs["active_palette_buffer"] = GetHexString(revisionData.ActivePaletteBuffer);
                subs["palette_to_zero"] = revisionData.PaletteToZero.ToString();
            }

            return subs;
        }

        protected abstract string CategoryName { get; }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Category, CategoryName, "addresses.json");
        }

        //TODO Move to PlatformProvider
        private string? GetPlatform(CameraInfo camera, CameraModelInfo cameraModel)
        {
            var modelId = camera?.Canon?.ModelId;
            if (modelId == null)
                return null;

            var models = cameraModel?.Names;
            if (models == null)
                return null;

            return PlatformGenerator.GetPlatform(modelId.Value, models, CategoryName, true);
        }

        private string? GetRevision(CameraInfo camera)
        {
            return FirmwareProvider.GetFirmwareRevision(camera, CategoryName);
        }

        private static string GetHexString<T>(T value)
        {
            return $"0x{value:x}";
        }
    }
}
