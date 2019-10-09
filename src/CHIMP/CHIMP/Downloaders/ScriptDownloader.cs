using Chimp.Model;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk;
using Net.Chdk.Generators.Script;
using Net.Chdk.Json;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Downloaders
{
    sealed class ScriptDownloader : DownloaderBase
    {
        private const string CategoryName = "SCRIPT";

        private string ProductName { get; }
        private IBootProvider BootProvider { get; }
        private IScriptGenerator ScriptGenerator { get; }
        private IMetadataService MetadataService { get; }
        private IDictionary<string, object> Substitutes { get; }

        public ScriptDownloader(string productName, MainViewModel mainViewModel, ISupportedProvider supportedProvider, IBootProvider bootProvider, IScriptGenerator scriptGenerator, IMetadataService metadataService,
            IDictionary<string, object> substitutes, ILogger logger)
                : base(mainViewModel, supportedProvider, logger)
        {
            ProductName = productName;
            BootProvider = bootProvider;
            ScriptGenerator = scriptGenerator;
            MetadataService = metadataService;
            Substitutes = substitutes;
        }

        public override Task<SoftwareData> DownloadAsync(SoftwareInfo software, CancellationToken cancellationToken)
        {
            var result = Download(software);
            return Task.FromResult(result);
        }

        private SoftwareData Download(SoftwareInfo softwareInfo)
        {
            var software = GetSoftware(softwareInfo);

            if (!Substitutes.ContainsKey("revision"))
            {
                var data = GetMatchData();
                SetSupportedItems(data, software);
                return null;
            }

            var destPath = GenerateScript(software);
            software = MetadataService.Update(software, destPath, null, default);

            return new SoftwareData
            {
                Paths = new[] { destPath },
                Info = software,
            };
        }

        private MatchData GetMatchData()
        {
            if (Substitutes.TryGetValue("error", out string error))
                return new MatchData(error);

            Substitutes.TryGetValue("platforms", out IEnumerable<string> platforms);
            Substitutes.TryGetValue("revisions", out IEnumerable<string> revisions);
            return new MatchData(platforms, revisions, null);
        }

        private string GenerateScript(SoftwareInfo software)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "CHIMP");
            Directory.CreateDirectory(tempPath);

            var platform = software?.Camera?.Platform;
            var revision = software?.Camera?.Revision;
            var dirName = $"{ProductName}-{platform}-{revision}";
            var version = software?.Product?.Version;
            if (version != null)
            {
                dirName = $"{dirName}-{version}";
                var status = software?.Build?.Status;
                if (!string.IsNullOrEmpty(status))
                    dirName = $"{dirName}-{status}";
            }

            var dirPath = Path.Combine(tempPath, dirName);
            if (Directory.Exists(dirPath))
            {
                Logger.LogTrace("Skipping {0}", dirPath);
            }
            else
            {
                Directory.CreateDirectory(dirPath);
                var filePath = Path.Combine(dirPath, BootProvider.GetFileName(CategoryName));
                ScriptGenerator.GenerateScript(filePath, ProductName, Substitutes);
            }

            return dirPath;
        }

        private SoftwareInfo GetSoftware(SoftwareInfo softwareInfo)
        {
            var software = GetSoftware();
            software.Camera = softwareInfo.Camera;
            software.Model = softwareInfo.Model;
            return software;
        }

        private SoftwareInfo GetSoftware()
        {
            var filePath = Path.Combine(Directories.Data, Directories.Product, ProductName, "software.json");
            using (var stream = File.OpenRead(filePath))
            {
                return JsonObject.Deserialize<SoftwareInfo>(stream);
            }
        }
    }
}
