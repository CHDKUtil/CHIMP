using System.Linq;
using Chimp.Model;
using Chimp.Properties;
using Net.Chdk.Model.Software;

namespace Chimp.Providers.Supported
{
    sealed class SupportedRevisionProvider : IInnerSupportedProvider
    {
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
            return data.Revisions
                .Select(GetRevision)
                .ToArray();
        }

        public string GetTitle(MatchData data)
        {
            return data.Revisions.Count() > 1
                ? Resources.Download_SupportedFirmwares_Content
                : Resources.Download_SupportedFirmware_Content;
        }

        private static string GetRevision(string value)
        {
            switch (value.Length)
            {
                case 3:
                    return $"{value[0]}.{value[1]}.{value[2]}";
                case 4:
                    return string.Format(Resources.Camera_FirmwareVersion_Format, value[0], value[1], value[2], value[3] - 'a' + 1, 0, 0);
                default:
                    return null;
            }
        }
    }
}
