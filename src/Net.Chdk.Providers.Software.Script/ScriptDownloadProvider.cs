using System.Collections.Generic;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ScriptDownloadProvider : IDownloadProvider<ScriptMatchData, ScriptDownloadData>
    {
        public IEnumerable<ScriptDownloadData> GetDownloads(ScriptMatchData data, SoftwareInfo software)
        {
            var substitutes = data.Payload;
            if (substitutes != null)
            {
                var path = GetPath(software);
                yield return new ScriptDownloadData(substitutes, software, path);
            }
        }

        private static string GetPath(SoftwareInfo software)
        {
            var productName = software?.Product?.Name;
            var platform = software?.Camera?.Platform;
            var revision = software?.Camera?.Revision;
            var dirName = $"{productName}-{platform}-{revision}";
            var version = software?.Product?.Version;
            if (version != null)
            {
                dirName = $"{dirName}-{version}";
                var status = software?.Build?.Status;
                if (!string.IsNullOrEmpty(status))
                    dirName = $"{dirName}-{status!.ToUpper()}";
            }
            return $"{dirName}.zip";
        }
    }
}
