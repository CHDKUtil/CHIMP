using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Watchers.Volume;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Controllers
{
    sealed class InstallController : Controller<InstallController, InstallViewModel>
    {
        protected override bool CanSkipStep => MainViewModel.IsAborted || ViewModel.IsCompleted;
        protected override bool SkipStep => MainViewModel.IsAborted || base.SkipStep;

        private IInstallerProvider InstallerProvider { get; }
        private IVolumeWatcher VolumeWatcher { get; }

        public InstallController(IInstallerProvider installerProvider, IVolumeWatcher volumeWatcher,
            MainViewModel mainViewModel, IStepProvider stepProvider, string stepName, ILoggerFactory loggerFactory)
            : base(mainViewModel, stepProvider, stepName, loggerFactory)
        {
            InstallerProvider = installerProvider;
            VolumeWatcher = volumeWatcher;
        }

        public override async Task EnterStepAsync()
        {
            StepViewModel.CanGoBack = false;
            StepViewModel.CanContinue = false;

            if (!MainViewModel.IsAborted)
            {
                bool result = false;
                using (cts = new CancellationTokenSource())
                {
                    try
                    {
                        result = await InstallAsync(cts.Token);
                    }
                    finally
                    {
                        cts = null;
                    }
                }

                if (result)
                {
                    SetTitle(nameof(Resources.Install_Completed_Text));
                    ViewModel.IsCompleted = true;
                }
                else
                {
                    MainViewModel.IsError = true;
                    ViewModel.IsAborted = true;
                }
            }

            StepViewModel.CanContinue = true;
            TrySkipStep();
        }

        public override async Task LeaveStepAsync()
        {
            await base.LeaveStepAsync();
            if (ViewModel?.IsCompleted == true)
            {
                //MainViewModel.IsCompleted = true;
            }
            else //if (ViewModel.IsAborted)
            {
                MainViewModel.IsAborted = true;
            }
        }

        private async Task<bool> InstallAsync(CancellationToken cancellationToken)
        {
            ViewModel = await CreateViewModelAsync();

            var card = CardViewModel.SelectedItem.Info;
            var installer = InstallerProvider.GetInstaller(card.FileSystem);
            if (installer == null)
                return false;

            VolumeWatcher.Stop();

            return await installer.InstallAsync(cancellationToken);
        }

        private async Task<InstallViewModel> CreateViewModelAsync()
        {
            //TODO Move to InstallService
            var tempPaths = DownloadViewModel.Paths;
            var items = await Task.Run(() => tempPaths.SelectMany(GetFileSystemEntries).ToArray());
            return new InstallViewModel
            {
                Title = Resources.Install_Initializing_Text,
                ProgressMaximum = items.Length,
            };
        }

        private static string[] GetFileSystemEntries(string path)
        {
            return Directory.GetFileSystemEntries(path, "*.*", SearchOption.AllDirectories);
        }

        private void SetTitle(string title)
        {
            Logger.LogInformation(title);
            ViewModel.Title = title;
            ViewModel.FileName = string.Empty;
        }
    }
}
