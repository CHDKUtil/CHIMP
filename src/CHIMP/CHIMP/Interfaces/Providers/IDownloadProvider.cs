using Chimp.Model;
using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chimp
{
    interface IDownloadProvider
    {
        IEnumerable<DownloadData> GetDownloads(Match[] matches, SoftwareInfo info);
    }
}