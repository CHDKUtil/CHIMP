using Chimp.Model;
using Chimp.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Actions
{
    abstract class ActionBase : IAction
    {
        protected MainViewModel MainViewModel { get; }
        protected SoftwareViewModel? SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);
        protected CardViewModel? CardViewModel => CardViewModel.Get(MainViewModel);
        protected CameraViewModel? CameraViewModel => CameraViewModel.Get(MainViewModel);
        protected DownloadViewModel? DownloadViewModel => DownloadViewModel.Get(MainViewModel);

        protected ActionBase(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        public abstract string? DisplayName { get; }

        public virtual bool IsDefault => false;

        public virtual async Task<SoftwareData?> PerformAsync(CancellationToken token)
        {
            return await Task.Run(() => Perform(), token);
        }

        protected virtual SoftwareData? Perform()
        {
            throw new NotImplementedException();
        }

        protected void SetTitle(string title)
        {
            if (DownloadViewModel != null)
                DownloadViewModel.Title = title;
        }
    }
}
