using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Providers.Substitute
{
    abstract class CategorySubstituteProvider : DataProvider<Dictionary<string, AddressPlatformData>>, ICategorySubstituteProvider
    {
        public CategorySubstituteProvider(ILogger logger)
            : base(logger)
        {
        }

        public IDictionary<string, string>? GetSubstitutes(string platform, string revision)
        {
            if (!Data.TryGetValue(platform, out AddressPlatformData platformData))
                return null;
            if (platformData.Revisions == null || !platformData.Revisions.TryGetValue(revision, out AddressRevisionData revisionData))
                return null;

            return new Dictionary<string, string>
            {
                ["platform"] = platform,
                ["revision"] = revision,
                ["platform_id"] = GetHexString(platformData.Id),
                ["palette_buffer_ptr"] = GetHexString(revisionData.PaletteBufferPtr),
                ["active_palette_buffer"] = GetHexString(revisionData.ActivePaletteBuffer),
                ["palette_to_zero"] = revisionData.PaletteToZero.ToString(),
            };
        }

        protected abstract string CategoryName { get; }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Category, CategoryName, "addresses.json");
        }

        private static string GetHexString<T>(T value)
        {
            return $"0x{value:x}";
        }
    }
}
