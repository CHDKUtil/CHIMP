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
    sealed class CategorySubstituteProvider : DataProvider<Dictionary<string, AddressPlatformData>>, ICategorySubstituteProvider
    {
        private IPlatformGenerator PlatformGenerator { get; }
        private IFirmwareProvider FirmwareProvider { get; }
        private string CategoryName { get; }

        public CategorySubstituteProvider(IPlatformGenerator platformGenerator, IFirmwareProvider firmwareProvider, string categoryName, ILogger logger)
            : base(logger)
        {
            PlatformGenerator = platformGenerator;
            FirmwareProvider = firmwareProvider;
            CategoryName = categoryName;
        }

        public IDictionary<string, object>? GetSubstitutes(CameraInfo camera, CameraModelInfo cameraModel)
        {
            var platform = GetPlatform(camera, cameraModel);
            if (platform == null)
                return null;

            var revision = GetRevision(camera);
            if (revision == null)
                return null;

            var revisionStr = GetRevisionString(revision);
            if (revisionStr == null)
                return null;

            var name = GetModelName(camera, cameraModel);
            if (name == null)
                return null;

            if (!Data.TryGetValue(platform, out AddressPlatformData platformData))
                return null;

            var subs = new Dictionary<string, object>
            {
                ["model"] = name,
                ["platform"] = platform,
                ["platform_id"] = GetHexString(platformData.Id),
                ["platform_id_address"] = GetHexString(platformData.IdAddress),
                ["clear_overlays"] = platformData.ClearOverlay
            };

            if (platformData.Revisions == null)
            {
                subs["error"] = "Download_InvalidFormat_Text";
                return subs;
            }

            if (!platformData.Revisions.TryGetValue(revision, out AddressRevisionData revisionData))
            {
                subs["revisions"] = platformData.Revisions.Keys;
                return subs;
            }

            subs["revision"] = revision;
            subs["revision_str"] = revisionStr;
            subs["revision_str_address"] = GetHexString(revisionData.RevisionAddress);
            subs["palette_buffer_ptr"] = GetHexString(revisionData.PaletteBufferPtr);
            subs["active_palette_buffer"] = GetHexString(revisionData.ActivePaletteBuffer);
            subs["palette_to_zero"] = revisionData.PaletteToZero;

            return subs;
        }

        public IEnumerable<string> GetSupportedPlatforms()
        {
            return Data.Keys;
        }

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

        private string? GetRevisionString(string revision)
        {
            return FirmwareProvider.GetRevisionString(revision, CategoryName);
        }

        private string? GetModelName(CameraInfo camera, CameraModelInfo cameraModel)
        {
            return FirmwareProvider.GetModelName(camera, cameraModel);
        }

        private static string GetHexString<T>(T value)
        {
            return $"0x{value:x}";
        }
    }
}
