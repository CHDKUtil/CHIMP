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

namespace Chimp.Actions
{
    sealed class ClearOverlaysAction : ActionBase
    {
        private const string CategoryName = "SCRIPT";
        private const string ProductName = "clear_overlays";

        private IBootProvider BootProvider { get; }
        private IScriptGenerator ScriptGenerator { get; }
        private IMetadataService MetadataService { get; }
        private IDictionary<string, object> Substitutes { get; }
        private ILogger Logger { get; }

        public ClearOverlaysAction(MainViewModel mainViewModel, IBootProvider bootProvider, IScriptGenerator scriptGenerator, IMetadataService metadataService, IDictionary<string, object> substitutes, ILogger<ClearOverlaysAction> logger)
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
                DownloadViewModel.SupportedItems = GetSupportedItems(Substitutes).ToArray();
                DownloadViewModel.SupportedTitle = GetSupportedTitle(Substitutes);
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

        private static IEnumerable<string> GetSupportedItems(IDictionary<string, object> subs)
        {
            if (subs["revisions"] is IEnumerable<string> revisions)
                return GetSupportedRevisions(revisions);
            if (subs["platforms"] is IEnumerable<string> platforms)
                return GetSupportedModels(platforms);
            return null;
        }

        private static string GetSupportedTitle(IDictionary<string, object> subs)
        {
            if (subs["revisions"] is IEnumerable<string> revisions)
                return GetSupportedRevisionsTitle(revisions);
            if (subs["platforms"] is IEnumerable<string> platforms)
                return GetSupportedModelsTitle(platforms);
            return null;
        }

        private static IEnumerable<string> GetSupportedModels(IEnumerable<string> _)
        {
            return Enumerable.Empty<string>();
        }

        private static string GetSupportedModelsTitle(IEnumerable<string> platforms)
        {
            return platforms.Count() > 1
                ? Resources.Download_SupportedModels_Content
                : Resources.Download_SupportedModel_Content;
        }

        private static IEnumerable<string> GetSupportedRevisions(IEnumerable<string> revisions)
        {
            return revisions.Select(GetRevision);
        }

        private static string GetSupportedRevisionsTitle(IEnumerable<string> revisions)
        {
            return revisions.Count() > 1
                ? Resources.Download_SupportedFirmwares_Content
                : Resources.Download_SupportedFirmware_Content;
        }

        private static string GetRevision(string value)
        {
            switch (value.Length)
            {
                case 3:
                    return $"{value[0]}.{value[1]}.{value[2]}";
                case 4:
                    return string.Format(Resources.Camera_FirmwareVersion_Format, value[0], value[1], value[2], value[3] - 'a' + 1, 0, 0);
                default:
                    return null;
            }
        }

        private string Platform => Substitutes["platform"] as string;
        private string Revision => Substitutes["revision"] as string;
    }
}
