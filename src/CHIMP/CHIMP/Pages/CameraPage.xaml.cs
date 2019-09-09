using Chimp.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Net.Chdk.Detectors.Camera;
using System.Collections.Generic;
using System.Text;
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
        private IEnumerable<IFilePatternProvider> FilePatternProviders { get; }

        public CameraPage(IControllerContainer controllerProvider, ILoggerFactory loggerFactory, IEnumerable<IFilePatternProvider> filePatternProviders)
        {
            Logger = loggerFactory.CreateLogger<CameraPage>();
            ControllerProvider = controllerProvider;
            FilePatternProviders = filePatternProviders;

            InitializeComponent();
        }

        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogTrace("Browse clicked");

            var dlg = new OpenFileDialog
            {
                Title = Properties.Resources.Camera_SelectImage_Text,
                Filter = GetFilter()
            };
            if (dlg.ShowDialog() == true)
            {
                await DetectFromBrowsedFileAsync(dlg.FileName);
            }
        }

        private string GetFilter()
        {
            var sb = new StringBuilder();
            foreach (var provider in FilePatternProviders)
            {
                var key = $"Camera_{provider.PatternsDescription}_Text";
                var description = Properties.Resources.ResourceManager.GetString(key);
                sb.Append(description);
                sb.Append('|');
                var patterns = string.Join(";", provider.Patterns);
                sb.Append(patterns);
                sb.Append('|');
            }
            sb.Append(Properties.Resources.Camera_AllFiles_Text);
            sb.Append("|*.*");
            return sb.ToString();
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
