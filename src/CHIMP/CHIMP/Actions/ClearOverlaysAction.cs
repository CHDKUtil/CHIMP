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

namespace Chimp.Actions
{
    sealed class ClearOverlaysAction : ActionBase
    {
        private const string CategoryName = "SCRIPT";
        private const string ProductName = "clear_overlays";

        private IBootProvider BootProvider { get; }
        private IScriptGenerator ScriptGenerator { get; }
        private IMetadataService MetadataService { get; }
        private IDictionary<string, string> Substitutes { get; }
        private ILogger Logger { get; }

        public ClearOverlaysAction(MainViewModel mainViewModel, IBootProvider bootProvider, IScriptGenerator scriptGenerator, IMetadataService metadataService, IDictionary<string, string> substitutes, ILogger<ClearOverlaysAction> logger)
            : base(mainViewModel)
        {
            BootProvider = bootProvider;
            ScriptGenerator = scriptGenerator;
            MetadataService = metadataService;
            Substitutes = substitutes;
            Logger = logger;
        }

        public override string DisplayName => Resources.Action_ClearOverlays_Text;

        protected override SoftwareData Perform()
        {
            if (!Substitutes.ContainsKey("revision"))
            {
                SetTitle(Resources.Download_UnsupportedFirmware_Text, LogLevel.Error);
                return null;
            }

            var software = GetSoftware();
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

            var dirName = $"{ProductName}-{Platform}-{Revision}";
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

        private SoftwareInfo GetSoftware()
        {
            var filePath = Path.Combine(Directories.Data, Directories.Product, ProductName, "software.json");
            using (var stream = File.OpenRead(filePath))
            {
                return JsonObject.Deserialize<SoftwareInfo>(stream);
            }
        }

        private void SetTitle(string title, LogLevel logLevel = LogLevel.Information)
        {
            Logger.Log(logLevel, default, title, null, null);
            DownloadViewModel.Title = title;
            DownloadViewModel.FileName = string.Empty;
        }

        private string Platform => Substitutes["platform"];
        private string Revision => Substitutes["revision"];
    }
}
