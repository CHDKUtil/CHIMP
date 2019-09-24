using Chimp.Model;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chimp.Providers.Downloads
{
    sealed class SdmDownloadProvider : DownloadProvider
    {
        private const string ProductName = "SDM";
        private const string CommonRoot = "Common_Files";

        public SdmDownloadProvider(Uri baseUri)
            : base(baseUri)
        {
        }

        public override IEnumerable<DownloadData> GetDownloads(Match[] matches, SoftwareInfo info)
        {
            yield return GetCommonDownload(matches[0], info);
            yield return GetDownload(matches[1]);
        }

        private DownloadData GetCommonDownload(Match commonMatch, SoftwareInfo info)
        {
            var version = info.Product?.Version;
            var download = GetDownload(commonMatch);
            download.TargetPath = $"{ProductName}-{version}-{download.Path}";
            download.RootDir = CommonRoot;
            return download;
        }
    }
}
