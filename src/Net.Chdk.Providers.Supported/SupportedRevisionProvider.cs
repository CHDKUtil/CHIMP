using System;
using System.Linq;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Software;

namespace Net.Chdk.Providers.Supported
{
    sealed class SupportedRevisionProvider : IInnerSupportedProvider
    {
        private IFirmwareProvider FirmwareProvider { get; }

        public SupportedRevisionProvider(IFirmwareProvider firmwareProvider)
        {
            FirmwareProvider = firmwareProvider;
        }

        public bool IsMatch(IMatchData data)
        {
            return data.Revisions != null;
        }

        public string GetError(IMatchData data)
        {
            return "Download_UnsupportedFirmware_Text";
        }

        public string[]? GetItems(IMatchData data, SoftwareInfo software)
        {
            var categoryName = GetCategoryName(software);
            if (categoryName == null)
                throw new InvalidOperationException("Unknown category");
            return data.Revisions
                .Select(r => GetRevision(r, categoryName)!)
                .ToArray();
        }

        public string GetTitle(IMatchData data)
        {
            return data.Revisions.Count() > 1
                ? "Download_SupportedFirmwares_Content"
                : "Download_SupportedFirmware_Content";
        }

        private string? GetCategoryName(SoftwareInfo software)
        {
            return FirmwareProvider.GetCategoryName(software.Camera);
        }

        private string? GetRevision(string value, string categoryName)
        {
            return FirmwareProvider.GetRevisionString(value, categoryName);
        }
    }
}
