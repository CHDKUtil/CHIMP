using Net.Chdk.Model.Software;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    public interface IDownloadProvider
    {
    }

    public interface IDownloadProvider<TMatchData, TDownloadData> : IDownloadProvider
        where TMatchData : IMatchData
        where TDownloadData : IDownloadData
    {
        IEnumerable<TDownloadData> GetDownloads(TMatchData data, SoftwareInfo software);
    }
}
