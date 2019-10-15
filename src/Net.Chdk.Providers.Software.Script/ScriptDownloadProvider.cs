using System.Collections.Generic;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ScriptDownloadProvider : IDownloadProvider
    {
        public IEnumerable<IDownloadData> GetDownloads(ISoftwareData software)
        {
            if (software.Match is ScriptMatchData data)
            {
                var path = GetPath(software.Info);
                var substitutes = data.Payload;
                if (substitutes != null)
                    yield return new ScriptDownloadData(substitutes, software.Info, path);
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
