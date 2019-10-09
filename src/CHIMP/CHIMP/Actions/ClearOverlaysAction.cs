using Chimp.Downloaders;
using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Script;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Actions
{
    sealed class ClearOverlaysAction : ActionBase
    {
        private const string ProductName = "clear_overlays";

        private IBootProvider BootProvider { get; }
        private IScriptGenerator ScriptGenerator { get; }
        private IMetadataService MetadataService { get; }

        private SoftwareInfo Software { get; }
        private IDictionary<string, object> Substitutes { get; }
        private ILogger Logger { get; }

        public ClearOverlaysAction(MainViewModel mainViewModel, IBootProvider bootProvider, IScriptGenerator scriptGenerator, IMetadataService metadataService, SoftwareInfo software, IDictionary<string, object> substitutes, ILogger<ClearOverlaysAction> logger)
            : base(mainViewModel)
        {
            BootProvider = bootProvider;
            ScriptGenerator = scriptGenerator;
            MetadataService = metadataService;
            Software = software;
            Substitutes = substitutes;
            Logger = logger;
        }

        public override string DisplayName => Resources.Action_ClearOverlays_Text;

        public override Task<SoftwareData> PerformAsync(CancellationToken token)
        {
            return GetDownloader().DownloadAsync(Software, token);
        }

        private IDownloader GetDownloader()
        {
            return new ScriptDownloader(ProductName, MainViewModel, BootProvider, ScriptGenerator, MetadataService, Substitutes, Logger);
        }
    }
}
