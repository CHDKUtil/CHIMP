using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers.Substitute
{
    sealed class CategorySubstituteProvider : DataProvider<Dictionary<string, AddressPlatformData>>, ICategorySubstituteProvider
    {
        private string CategoryName { get; }
        private IPlatformProvider PlatformProvider { get; }
        private IFirmwareProvider FirmwareProvider { get; }

        public CategorySubstituteProvider(string categoryName, IPlatformProvider platformProvider, IFirmwareProvider firmwareProvider, ILogger logger)
            : base(logger)
        {
            CategoryName = categoryName;
            PlatformProvider = platformProvider;
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
            subs["model_id"] = GetHexString(camera.Canon.ModelId);

            if (platformData.Revisions == null)
            {
                subs["error"] = "Download_InvalidFormat_Text";
                return subs;
            }

            if (!platformData.Revisions.TryGetValue(revision, out AddressRevisionData revisionData))
            {
                subs["revisions"] = platformData.Revisions.Select(kvp => kvp.Key);
                return subs;
            }

            subs["revision"] = revision;
            subs["palette_buffer_ptr"] = GetHexString(revisionData.PaletteBufferPtr);
            subs["active_palette_buffer"] = GetHexString(revisionData.ActivePaletteBuffer);
            subs["palette_to_zero"] = revisionData.PaletteToZero;

            return subs;
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Category, CategoryName, "addresses.json");
        }

        private string? GetPlatform(CameraInfo camera, CameraModelInfo cameraModel)
        {
            return PlatformProvider.GetPlatform(camera, cameraModel, CategoryName);
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
