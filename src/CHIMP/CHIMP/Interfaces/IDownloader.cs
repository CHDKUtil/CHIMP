using Chimp.Model;
using Net.Chdk.Model.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp
{
    interface IDownloader
    {
        Task<SoftwareData> DownloadAsync(SoftwareInfo software, CancellationToken cancellationToken);
    }
}