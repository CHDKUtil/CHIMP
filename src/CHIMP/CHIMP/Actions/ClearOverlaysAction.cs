using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk;
using Net.Chdk.Generators.Script;
using Net.Chdk.Json;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chimp.Actions
{
    sealed class ClearOverlaysAction : ActionBase
    {
        private const string ProductName = "clear_overlays";

        private static readonly Version ProductVersion = new Version("1.0");

        private IScriptGenerator ScriptGenerator { get; }
        private IMetadataService MetadataService { get; }
        private IDictionary<string, string> Substitutes { get; }
        private ILogger Logger { get; }

        public ClearOverlaysAction(MainViewModel mainViewModel, IScriptGenerator scriptGenerator, IMetadataService metadataService, IDictionary<string, string> substitutes, ILogger<ClearOverlaysAction> logger)
            : base(mainViewModel)
        {
            MetadataService = metadataService;
            ScriptGenerator = scriptGenerator;
            Substitutes = substitutes;
            Logger = logger;
        }

        public override string DisplayName => Resources.Action_ClearOverlays_Text;

        protected override SoftwareData Perform()
        {
            var destPath = GenerateScript();
            var software = GetSoftware();
            MetadataService.Update(software, destPath, null, default);

            return new SoftwareData
            {
                Paths = new[] { destPath },
                Info = software,
            };
        }

        private string GenerateScript()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "CHIMP");
            Directory.CreateDirectory(tempPath);

            var dirName = $"{ProductName}-{Platform}-{Revision}-{ProductVersion}";
            var dirPath = Path.Combine(tempPath, dirName);
            if (Directory.Exists(dirPath))
            {
                Logger.LogTrace("Skipping {0}", dirPath);
            }
            else
            {
                Directory.CreateDirectory(dirPath);
                ScriptGenerator.GenerateScript(dirPath, ProductName, Substitutes);
            }

            return dirPath;
        }

        private SoftwareInfo GetSoftware()
        {
            SoftwareInfo software;
            var filePath = Path.Combine(Directories.Data, Directories.Product, ProductName, "software.json");
            using (var stream = File.OpenRead(filePath))
            {
                software = JsonObject.Deserialize<SoftwareInfo>(stream);
            }
            software.Camera = GetCamera();
            return software;
        }

        private SoftwareCameraInfo GetCamera()
        {
            return new SoftwareCameraInfo
            {
                Platform = Platform,
                Revision = Revision,
            };
        }

        private string Platform => Substitutes["platform"];
        private string Revision => Substitutes["revision"];
    }
}
