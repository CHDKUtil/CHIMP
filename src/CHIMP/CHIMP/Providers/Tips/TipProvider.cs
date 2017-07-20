using Chimp.Model;
using Chimp.ViewModels;
using System.Collections.Generic;

namespace Chimp.Providers.Tips
{
    abstract class TipProvider : ITipProvider
    {
        protected MainViewModel MainViewModel { get; }

        protected TipProvider(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        public abstract IEnumerable<Tip> GetTips(string productText);

        protected CameraViewModel CameraViewModel => CameraViewModel.Get(MainViewModel);
        protected DownloadViewModel DownloadViewModel => DownloadViewModel.Get(MainViewModel);
        protected SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);

        protected string ProductName =>
            DownloadViewModel?.Software?.Product.Name
            ?? SoftwareViewModel?.SelectedItem?.Info.Product.Name;
    }
}
