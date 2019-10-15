using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    public interface IDownloadProvider
    {
        IEnumerable<IDownloadData>? GetDownloads(ISoftwareData software);
    }
}
