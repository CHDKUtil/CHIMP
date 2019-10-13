using System.Collections.Generic;
using Chimp.Model;
using Net.Chdk.Model.Software;

namespace Chimp.Providers.Downloads
{
    sealed class ScriptDownloadProvider : IDownloadProvider
    {
        public IEnumerable<DownloadData> GetDownloads(SoftwareData software)
        {
            if (software.Match is ScriptMatchData data)
            {
                var path = GetPath(software.Info);
                yield return new ScriptDownloadData(data.Substitutes, software.Info, path);
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
                    dirName = $"{dirName}-{status.ToUpper()}";
            }
            return $"{dirName}.zip";
        }
    }
}
