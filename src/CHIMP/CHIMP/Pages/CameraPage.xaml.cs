using Chimp.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Net.Chdk.Detectors.Camera;
using System.Threading.Tasks;
using System.Windows;

namespace Chimp.Pages
{
    /// <summary>
    /// Interaction logic for CameraPage.xaml
    /// </summary>
    public partial class CameraPage
    {
        private ILogger Logger { get; }
        private IControllerContainer ControllerProvider { get; }

        public CameraPage(IControllerContainer controllerProvider, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<CameraPage>();
            ControllerProvider = controllerProvider;

            InitializeComponent();
        }

        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogTrace("Browse clicked");

            var canonFilter = string.Join(";", FileSystemCameraDetector.Patterns);
            var filter = $"{Properties.Resources.Camera_CanonImages_Text}|{canonFilter}|{Properties.Resources.Camera_AllFiles_Text}|*.*";
            var title = Properties.Resources.Camera_SelectImage_Text;
            var dlg = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };
            if (dlg.ShowDialog() == true)
            {
                await DetectFromBrowsedFileAsync(dlg.FileName);
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            var fileNames = e.Data.GetData("FileName") as string[];
            if (fileNames?.Length == 1)
                e.Effects = DragDropEffects.Link;
            else
                e.Effects = DragDropEffects.None;
        }

        protected override async void OnDrop(DragEventArgs e)
        {
            var fileNames = e.Data.GetData("FileName") as string[];
            if (fileNames?.Length == 1)
            {
                await DetectFromDroppedFileAsync(fileNames[0]);
            }
        }

        private async Task DetectFromBrowsedFileAsync(string path)
        {
            Logger.LogInformation("Browsed {0}", path);
            var controller = await GetControllerAsync();
            await controller.DetectCameraAsync(path);
        }

        private async Task DetectFromDroppedFileAsync(string path)
        {
            Logger.LogInformation("Dropped {0}", path);
            var controller = await GetControllerAsync();
            await controller.DetectCameraAsync(path);
        }

        private async Task<CameraController> GetControllerAsync()
        {
            return (CameraController)await ControllerProvider.GetControllerAsync("Camera");
        }
    }
}
