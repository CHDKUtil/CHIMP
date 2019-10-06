using Chimp.Downloaders;
using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Script;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Actions
{
    sealed class ClearOverlaysAction : ActionBase
    {
        private const string ProductName = "clear_overlays";

        private ICameraProvider CameraProvider { get; }
        private IBootProvider BootProvider { get; }
        private IScriptGenerator ScriptGenerator { get; }
        private IMetadataService MetadataService { get; }

        private SoftwareCameraInfo Camera { get; }
        private IDictionary<string, object> Substitutes { get; }
        private ILogger Logger { get; }

        public ClearOverlaysAction(MainViewModel mainViewModel, ICameraProvider cameraProvider, IBootProvider bootProvider, IScriptGenerator scriptGenerator, IMetadataService metadataService, SoftwareCameraInfo camera, IDictionary<string, object> substitutes, ILogger<ClearOverlaysAction> logger)
            : base(mainViewModel)
        {
            CameraProvider = cameraProvider;
            BootProvider = bootProvider;
            ScriptGenerator = scriptGenerator;
            MetadataService = metadataService;
            Camera = camera;
            Substitutes = substitutes;
            Logger = logger;
        }

        public override string DisplayName => Resources.Action_ClearOverlays_Text;

        public override Task<SoftwareData> PerformAsync(CancellationToken token)
        {
            var software = SoftwareViewModel?.SelectedItem?.Info;
            return GetDownloader().DownloadAsync(Camera, software, token);
        }

        private IDownloader GetDownloader()
        {
            return new ScriptDownloader(ProductName, MainViewModel, CameraProvider, BootProvider, ScriptGenerator, MetadataService, Substitutes, Logger);
        }
    }
}
