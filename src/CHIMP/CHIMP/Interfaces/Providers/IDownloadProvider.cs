using Chimp.Model;
using System.Collections.Generic;

namespace Chimp
{
    interface IDownloadProvider
    {
        IEnumerable<DownloadData> GetDownloads(SoftwareData software);
    }
}