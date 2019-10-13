using Chimp.Model;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Script;
using Net.Chdk.Providers.Boot;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    sealed class ScriptDownloader : DownloaderBase
    {
        private const string CategoryName = "SCRIPT";

        private IBootProvider BootProvider { get; }
        private IScriptGenerator ScriptGenerator { get; }

        public ScriptDownloader(MainViewModel mainViewModel, ISupportedProvider supportedProvider, IBuildProvider buildProvider,
            IMatchProvider matchProvider, ISoftwareProvider softwareProvider, IDownloadProvider downloadProvider, IBootProvider bootProvider,
            IScriptGenerator scriptGenerator, IMetadataService metadataService, ILogger<ScriptDownloader> logger)
                : base(mainViewModel, buildProvider, matchProvider, softwareProvider, downloadProvider, metadataService, supportedProvider, logger)
        {
            BootProvider = bootProvider;
            ScriptGenerator = scriptGenerator;
        }

        protected override Task<ExtractData> DownloadAsync(DownloadData download, string path, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            var filePath = Download(download, dirPath: dirPath);
            return Task.FromResult(filePath);
        }

        protected override Task<string> ExtractAsync(ExtractData extract, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            var result = Extract(extract, dirPath: dirPath);
            return Task.FromResult(result);
        }

        private string Extract(ExtractData extract, string dirPath)
        {
            if (!(extract is ScriptExtractData data))
                return null;
            ScriptGenerator.GenerateScript(extract.FilePath, data.ProductName, data.Substitutes);
            var files = BootProvider.GetFiles(CategoryName);
            foreach (var kvp in files)
            {
                var path = Path.Combine(dirPath, kvp.Key);
                File.WriteAllBytes(path, kvp.Value);
            }
            return dirPath;
        }

        private ExtractData Download(DownloadData download, string dirPath)
        {
            if (!(download is ScriptDownloadData data))
                return null;

            Directory.CreateDirectory(dirPath);

            var fileName = BootProvider.GetFileName(CategoryName);
            var filePath = Path.Combine(dirPath, fileName);

            var productName = data.Software.Product.Name;
            return new ScriptExtractData(data.Substitutes, productName: productName, filePath: filePath);
        }
    }
}
