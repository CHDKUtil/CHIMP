using Chimp.Properties;
using Chimp.ViewModels;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;

namespace Chimp.Services
{
    sealed class ExtractService : IExtractService
    {
        private ILogger Logger { get; }
        private MainViewModel MainViewModel { get; }
        private DownloadViewModel ViewModel => DownloadViewModel.Get(MainViewModel);

        public ExtractService(MainViewModel mainViewModel, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<ExtractService>();
            MainViewModel = mainViewModel;
        }

        public string Extract(string path, string filePath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            var tempDirPath = Extract(filePath, tempPath, cancellationToken);
            if (tempDirPath == null)
                return null;

            Directory.Move(tempDirPath, dirPath);

            return dirPath;
        }

        private string Extract(string filePath, string tempPath, CancellationToken cancellationToken)
        {
            var tempDirPath = Path.Combine(tempPath, Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirPath);

            Logger.LogTrace("Extracting {0} into {1}", filePath, tempDirPath);

            using (var fileStream = File.OpenRead(filePath))
            using (var zipStream = new ZipInputStream(fileStream))
            {
                var buffer = new byte[Settings.Default.ExtractBufferSize];
                ZipEntry entry;
                while ((entry = zipStream.GetNextEntry()) != null)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return null;

                    var outPath = Path.Combine(tempDirPath, entry.Name);
                    var fileName = Path.GetFileName(outPath);
                    if (fileName.Length == 0)
                        ExtractDirectory(entry, outPath);
                    else
                        ExtractFile(zipStream, entry, outPath, buffer);
                }
            }

            return tempDirPath;
        }

        private void ExtractDirectory(ZipEntry entry, string outPath)
        {
            Logger.LogTrace("Extracting directory {0}", entry.Name);

            ViewModel.FileName = entry.Name;

            CreateDirectory(outPath);

            Directory.SetCreationTimeUtc(outPath, entry.DateTime);
        }

        private void ExtractFile(ZipInputStream zipStream, ZipEntry entry, string outPath, byte[] buffer)
        {
            Logger.LogTrace("Extracting file {0}", entry.Name);

            ViewModel.FileName = entry.Name;

            var dirPath = Path.GetDirectoryName(outPath);
            CreateDirectory(dirPath);

            using (var outStream = File.Create(outPath))
            {
                StreamUtils.Copy(zipStream, outStream, buffer);
            }

            File.SetCreationTimeUtc(outPath, entry.DateTime);
        }

        private void CreateDirectory(string dir)
        {
            if (dir.Length == 0 || Directory.Exists(dir))
                return;

            dir = dir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            Logger.LogTrace("Creating {0}", dir);

            var parent = Path.GetDirectoryName(dir);
            CreateDirectory(parent);
            Directory.CreateDirectory(dir);
        }
    }
}
