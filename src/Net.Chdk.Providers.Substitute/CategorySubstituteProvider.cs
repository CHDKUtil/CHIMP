using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Providers.Firmware;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            var subs = new Dictionary<string, object>
            {
                ["model"] = cameraModel.Names[0],
            };

            if (!Data.TryGetValue(platform, out AddressPlatformData platformData))
            {
                subs["platforms"] = Data.Select(kvp => kvp.Key);
                return subs;
            }

            subs["platform"] = platform;
            subs["platform_id"] = GetHexString(platformData.Id);
            subs["platform_id_address"] = GetHexString(platformData.IdAddress);
            subs["clear_overlays"] = platformData.ClearOverlay;

            if (platformData.Revisions == null)
            {
                return subs;
            }

            if (!platformData.Revisions.TryGetValue(revision, out AddressRevisionData revisionData))
            {
                subs["revisions"] = platformData.Revisions.Select(kvp => kvp.Key);
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

        private string? GetRevisionString(string revision)
        {
            return FirmwareProvider.GetRevisionString(revision, CategoryName);
        }

        private static string GetHexString<T>(T value)
        {
            return $"0x{value:x}";
        }
    }
}
