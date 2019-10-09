using Chimp.Model;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk;
using Net.Chdk.Generators.Script;
using Net.Chdk.Json;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Substitute;
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
        private ISubstituteProvider SubstituteProvider { get; }

        public ScriptDownloader(string productName, MainViewModel mainViewModel, ISupportedProvider supportedProvider, IBootProvider bootProvider, IScriptGenerator scriptGenerator, IMetadataService metadataService,
            ISubstituteProvider substituteProvider, ILogger<ScriptDownloader> logger)
                : base(mainViewModel, metadataService, supportedProvider, logger)
        {
            ProductName = productName;
            BootProvider = bootProvider;
            ScriptGenerator = scriptGenerator;
            SubstituteProvider = substituteProvider;
        }

        protected override Task<SoftwareData> GetSoftwareAsync(SoftwareInfo softwareInfo, CancellationToken _)
        {
            var result = GetSoftware(softwareInfo);
            return Task.FromResult<SoftwareData>(result);
        }

        protected override Task<string[]> DownloadExtractAsync(SoftwareData softwareData, CancellationToken _)
        {
            var result = DownloadExtract(softwareData);
            return Task.FromResult(result);
        }

        private SoftwareData GetSoftware(SoftwareInfo softwareInfo)
        {
            return new SoftwareData
            {
                Info = GetSoftwareInfo(softwareInfo)
            };
        }

        private string[] DownloadExtract(SoftwareData softwareData)
        {
            var software = softwareData.Info;
            var substitutes = SubstituteProvider.GetSubstitutes(software);
            if (!substitutes.ContainsKey("revision"))
            {
                var data = GetMatchData(substitutes);
                SetSupportedItems(data, software);
                return null;
            }

            var destPath = GenerateScript(software, substitutes);
            return new[] { destPath };
        }

        private MatchData GetMatchData(IDictionary<string, object> substitutes)
        {
            if (substitutes.TryGetValue("error", out string error))
                return new MatchData(error);

            substitutes.TryGetValue("platforms", out IEnumerable<string> platforms);
            substitutes.TryGetValue("revisions", out IEnumerable<string> revisions);
            return new MatchData(platforms, revisions, null);
        }

        private string GenerateScript(SoftwareInfo software, IDictionary<string, object> substitutes)
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
                ScriptGenerator.GenerateScript(filePath, ProductName, substitutes);
            }

            return dirPath;
        }

        private SoftwareInfo GetSoftwareInfo(SoftwareInfo softwareInfo)
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
