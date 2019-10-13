using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk;
using Net.Chdk.Generators.Script;
using Net.Chdk.Json;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ScriptDownloader(string productName, MainViewModel mainViewModel, IBootProvider bootProvider, IScriptGenerator scriptGenerator, IMetadataService metadataService,
            IDictionary<string, object> substitutes, ILogger logger)
                : base(mainViewModel, logger)
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
            if (!Substitutes.ContainsKey("revision"))
            {
                var error = GetError(Substitutes);
                SetTitle(error, LogLevel.Error);
                ViewModel.SupportedItems = GetSupportedItems(Substitutes).ToArray();
                ViewModel.SupportedTitle = GetSupportedTitle(Substitutes);
                return null;
            }

            var software = GetSoftware(softwareInfo);
            var destPath = GenerateScript(software);
            software = MetadataService.Update(software, destPath, null, default);

            return new SoftwareData
            {
                Paths = new[] { destPath },
                Info = software,
            };
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
                GenerateScript(dirPath);
            }

            return dirPath;
        }

        private void GenerateScript(string dirPath)
        {
            var filePath = Path.Combine(dirPath, BootProvider.GetFileName(CategoryName));
            ScriptGenerator.GenerateScript(filePath, ProductName, Substitutes);
            var files = BootProvider.GetFiles(CategoryName);
            foreach (var kvp in files)
            {
                var path = Path.Combine(dirPath, kvp.Key);
                File.WriteAllBytes(path, kvp.Value);
            }
        }

        private SoftwareInfo GetSoftware(SoftwareInfo softwareInfo)
        {
            var software = GetSoftware();
            software.Camera = softwareInfo.Camera;
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

        private static string GetError(IDictionary<string, object> subs)
        {
            if (subs.ContainsKey("revisions"))
                return Resources.Download_UnsupportedFirmware_Text;
            if (subs.ContainsKey("platforms"))
                return Resources.Download_UnsupportedModel_Text;
            return null;
        }

        private static IEnumerable<string> GetSupportedItems(IDictionary<string, object> subs)
        {
            if (TryGetValue(subs, "revisions", out var revisions))
                return GetSupportedRevisions(revisions);
            if (TryGetValue(subs, "platforms", out var platforms))
                return GetSupportedModels(platforms);
            return null;
        }

        private static string GetSupportedTitle(IDictionary<string, object> subs)
        {
            if (TryGetValue(subs, "revisions", out var revisions))
                return GetSupportedRevisionsTitle(revisions);
            if (TryGetValue(subs, "platforms", out var platforms))
                return GetSupportedModelsTitle(platforms);
            return null;
        }

        private static bool TryGetValue(IDictionary<string, object> subs, string key, out IEnumerable<string> values)
        {
            if (!subs.TryGetValue(key, out object value))
            {
                values = null;
                return false;
            }
            values = value as IEnumerable<string>;
            return values != null;
        }
    }
}
