using Chimp.Model;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Script;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Software.Script;
using Net.Chdk.Providers.Supported;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    sealed class ScriptDownloader : Downloader<ScriptMatchData, ScriptDownloadData, ScriptExtractData, IDictionary<string, object>>
    {
        private const string CategoryName = "SCRIPT";

        private IBootProvider BootProvider { get; }
        private IScriptGenerator ScriptGenerator { get; }

        public ScriptDownloader(MainViewModel mainViewModel, ISupportedProvider supportedProvider,
            ScriptBuildProvider buildProvider, ScriptMatchProvider matchProvider, ScriptSoftwareProvider softwareProvider, ScriptDownloadProvider downloadProvider,
            IBootProvider bootProvider, IScriptGenerator scriptGenerator, IMetadataService metadataService, ILogger<ScriptDownloader> logger)
                : base(mainViewModel, buildProvider, matchProvider, softwareProvider, downloadProvider, metadataService, supportedProvider, logger)
        {
            BootProvider = bootProvider;
            ScriptGenerator = scriptGenerator;
        }

        protected override Task<ScriptExtractData> DownloadAsync(ScriptDownloadData download, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            var filePath = Download(download, dirPath: dirPath);
            return Task.FromResult(filePath);
        }

        protected override Task<string> ExtractAsync(ScriptExtractData extract, string targetPath, string dirPath, string tempPath, CancellationToken cancellationToken)
        {
            var result = Extract(extract, dirPath: dirPath);
            return Task.FromResult(result);
        }

        private ScriptExtractData Download(ScriptDownloadData download, string dirPath)
        {
            Directory.CreateDirectory(dirPath);

            var fileName = BootProvider.GetFileName(CategoryName);
            var filePath = Path.Combine(dirPath, fileName);

            var productName = download.Software.Product.Name;
            return new ScriptExtractData(download.Substitutes, productName: productName, filePath: filePath);
        }

        private string Extract(ScriptExtractData extract, string dirPath)
        {
            ScriptGenerator.GenerateScript(extract.FilePath, extract.ProductName, extract.Substitutes);
            var files = BootProvider.GetFiles(CategoryName);
            foreach (var kvp in files)
            {
                var path = Path.Combine(dirPath, kvp.Key);
                File.WriteAllBytes(path, kvp.Value);
            }
            return dirPath;
        }
    }
}
