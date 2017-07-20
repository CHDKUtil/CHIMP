using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Controllers
{
    sealed class DownloadController : Controller<DownloadController, DownloadViewModel>
    {
        protected override string StepName => "Download";
        protected override bool CanSkipStep => ViewModel.IsCompleted;
        protected override bool SkipStep => Settings.Default.SkipDownloadStep;

        private ICameraProvider CameraProvider { get; }

        public DownloadController(ICameraProvider cameraProvider, MainViewModel mainViewModel, ILoggerFactory loggerFactory)
            : base(mainViewModel, loggerFactory)
        {
            CameraProvider = cameraProvider;
        }

        public override async Task EnterStepAsync()
        {
            StepViewModel.CanGoBack = false;
            StepViewModel.CanContinue = false;

            if (!MainViewModel.IsAborted)
            {
                bool result;
                using (cts = new CancellationTokenSource())
                {
                    result = await DownloadAsync();
                }
                cts = null;

                if (result)
                {
                    ViewModel.Title = nameof(Resources.Download_Completed_Text);
                    ViewModel.FileName = string.Empty;
                    ViewModel.IsCompleted = true;
                }
                else
                {
                    MainViewModel.IsWarning = true;
                    ViewModel.IsAborted = true;
                }
            }

            StepViewModel.CanContinue = true;
            TrySkipStep();
        }

        public override async Task LeaveStepAsync()
        {
            await base.LeaveStepAsync();
            if (ViewModel.IsAborted)
            {
                MainViewModel.IsAborted = true;
            }
        }

        private async Task<bool> DownloadAsync()
        {
            ViewModel = CreateViewModel();

            var software = await DownloadAsync(cts.Token);
            if (software == null)
                return false;

            ViewModel.Software = software.Info;
            ViewModel.Paths = software.Paths;

            return true;
        }

        private async Task<SoftwareData> DownloadAsync(CancellationToken cancellationToken)
        {
            try
            {
                var action = ActionViewModel.SelectedItem.Action;
                return await action.PerformAsync(cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogError(0, ex, "Canceled");
                return null;
            }
            finally
            {
                cts = null;
            }
        }

        private DownloadViewModel CreateViewModel()
        {
            return new DownloadViewModel();
        }
    }
}
