using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers.Substitute
{
    sealed class CategorySubstituteProvider : DataProvider<Dictionary<string, AddressPlatformData>>, ICategorySubstituteProvider
    {
        private IFirmwareProvider FirmwareProvider { get; }
        private string CategoryName { get; }

        public CategorySubstituteProvider(IFirmwareProvider firmwareProvider, string categoryName, ILogger logger)
            : base(logger)
        {
            FirmwareProvider = firmwareProvider;
            CategoryName = categoryName;
        }

        public IDictionary<string, object>? GetSubstitutes(SoftwareInfo software)
        {
            var name = software.Model?.Names?[0];
            var modelId = software.Model?.Id;
            if (name == null || modelId == null)
                return null;

            var platform = software.Camera?.Platform;
            var revision = software.Camera?.Revision;
            if (platform == null || revision == null)
                return null;

            var revisionStr = GetRevisionString(revision);
            if (revisionStr == null)
                return null;

            var subs = new Dictionary<string, object>
            {
                ["model"] = name,
            };

            if (!Data.TryGetValue(platform, out AddressPlatformData platformData))
                return subs;

            subs["platform"] = platform;
            subs["platform_id"] = GetHexString(platformData.Id);
            subs["platform_id_address"] = GetHexString(platformData.IdAddress);
            subs["model_id"] = GetHexString(modelId);
            subs["clear_overlays"] = platformData.ClearOverlay;

            if (platformData.Revisions == null)
                return subs;

            if (!platformData.Revisions.TryGetValue(revision, out AddressRevisionData revisionData))
                return subs;

            subs["revision"] = revision;
            subs["revision_str"] = revisionStr;
            subs["revision_str_address"] = GetHexString(revisionData.RevisionAddress);
            subs["palette_buffer_ptr"] = GetHexString(revisionData.PaletteBufferPtr);
            subs["active_palette_buffer"] = GetHexString(revisionData.ActivePaletteBuffer);
            subs["palette_to_zero"] = revisionData.PaletteToZero;

            return subs;
        }

        public IEnumerable<string> GetSupportedPlatforms(SoftwareInfo software)
        {
            return Data.Keys;
        }

        public IEnumerable<string> GetSupportedRevisions(SoftwareInfo software)
        {
            var platformData = GetPlatformData(software);
            return platformData?.Revisions != null
                ? platformData.Revisions.Keys
                : Enumerable.Empty<string>();
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Category, CategoryName, "addresses.json");
        }

        private AddressPlatformData? GetPlatformData(SoftwareInfo software)
        {
            var platform = software.Camera?.Platform;
            if (platform == null || !Data.TryGetValue(platform, out AddressPlatformData platformData))
                return null;
            return platformData;
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
