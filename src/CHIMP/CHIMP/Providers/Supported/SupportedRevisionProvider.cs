using System.Linq;
using Chimp.Model;
using Chimp.Properties;
using Net.Chdk.Providers.Firmware;

namespace Chimp.Providers.Supported
{
    sealed class SupportedRevisionProvider : IInnerSupportedProvider
    {
        private IFirmwareProvider FirmwareProvider { get; }

        public SupportedRevisionProvider(IFirmwareProvider firmwareProvider)
        {
            FirmwareProvider = firmwareProvider;
        }

        public bool IsMatch(MatchData data)
        {
            return data.Revisions != null;
        }

        public string GetError(MatchData data)
        {
            return Resources.Download_UnsupportedFirmware_Text;
        }

        public string[] GetItems(MatchData data)
        {
            var categoryName = GetCategoryName(data);
            return data.Revisions
                .Select(r => GetRevision(r, categoryName))
                .ToArray();
        }

        public string GetTitle(MatchData data)
        {
            return data.Revisions.Count() > 1
                ? Resources.Download_SupportedFirmwares_Content
                : Resources.Download_SupportedFirmware_Content;
        }

        private string GetCategoryName(MatchData data)
        {
            return FirmwareProvider.GetCategoryName(data.Software.Camera);
        }

        private string GetRevision(string value, string categoryName)
        {
            return FirmwareProvider.GetRevisionString(value, categoryName);
        }
    }
}
