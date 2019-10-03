using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Platform;
using System.Collections.Generic;
using System.IO;

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
            if (platformData.Revisions == null || !platformData.Revisions.TryGetValue(revision, out AddressRevisionData revisionData))
                return null;

            return new Dictionary<string, string>
            {
                ["model"] = cameraModel.Names[0],
                ["platform"] = platform,
                ["revision"] = revision,
                ["platform_id"] = GetHexString(platformData.Id),
                ["palette_buffer_ptr"] = GetHexString(revisionData.PaletteBufferPtr),
                ["active_palette_buffer"] = GetHexString(revisionData.ActivePaletteBuffer),
                ["palette_to_zero"] = revisionData.PaletteToZero.ToString(),
            };
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
