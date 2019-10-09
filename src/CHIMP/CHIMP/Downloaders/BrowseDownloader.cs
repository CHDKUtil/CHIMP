using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chimp.Model;
using Net.Chdk.Model.Software;

namespace Chimp.Downloaders
{
    sealed class BrowseDownloader : IDownloader
    {
        public Task<SoftwareData> DownloadAsync(SoftwareInfo software, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
