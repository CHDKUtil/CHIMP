using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Services
{
    sealed class DownloadService : IDownloadService
    {
        private ILogger Logger { get; }
        private MainViewModel MainViewModel { get; }
        private DownloadViewModel ViewModel => DownloadViewModel.Get(MainViewModel);

        public DownloadService(MainViewModel mainViewModel, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<DownloadService>();
            MainViewModel = mainViewModel;
        }

        public async Task<string> DownloadAsync(Uri baseUri, string path, string filePath, CancellationToken cancellationToken)
        {
            var tempFilePath = await DownloadAsync(baseUri, path, cancellationToken);
            if (tempFilePath == null)
                return null;

            File.Move(tempFilePath, filePath);

            return filePath;
        }

        private async Task<string> DownloadAsync(Uri baseUri, string path, CancellationToken cancellationToken)
        {
            var ub = new UriBuilder(baseUri);
            ub.Path += path;
            var uri = ub.Uri;

            Logger.LogInformation("Downloading {0}", uri.OriginalString);

            using (var http = new HttpClient())
            using (var resp = await http.GetAsync(uri, cancellationToken))
            {
                try
                {
                    resp.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    Logger.LogError(0, ex, "Error downloading");
                    throw;
                }

                TryGetContentLength(resp, out int size);

                return await DownloadAsync(resp, size, cancellationToken);
            }
        }

        private async Task<string> DownloadAsync(HttpResponseMessage resp, int size, CancellationToken cancellationToken)
        {
            ViewModel.ProgressValue = 0;
            if (size > 0)
                ViewModel.ProgressMaximum = size;

            var fileName = Path.GetTempFileName();
            using (var respStream = await resp.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                var buffer = new byte[Settings.Default.DownloadBufferSize];
                int read;
                while ((read = await respStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, read, cancellationToken);
                    if (size > 0)
                        ViewModel.ProgressValue += read;
                }
            }

            return fileName;
        }

        private static bool TryGetContentLength(HttpResponseMessage resp, out int length)
        {
            if (resp.Content.Headers.ContentLength != null)
            {
                length = (int)resp.Content.Headers.ContentLength.Value;
                return true;
            }

            length = 0;
            return false;
        }
    }
}
