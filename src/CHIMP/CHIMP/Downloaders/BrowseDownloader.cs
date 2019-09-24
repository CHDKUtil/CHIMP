using System;
using System.Threading;
using System.Threading.Tasks;
using Chimp.Model;
using Net.Chdk.Model.Software;

namespace Chimp.Downloaders
{
    sealed class BrowseDownloader : IDownloader
    {
        public Task<SoftwareData?> DownloadAsync(SoftwareCameraInfo camera, SoftwareInfo software, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
