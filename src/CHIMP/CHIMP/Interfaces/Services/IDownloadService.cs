using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp
{
    interface IDownloadService
    {
        Task<string?> DownloadAsync(Uri? baseUri, string? path, string filePath, CancellationToken cancellationToken);
    }
}
