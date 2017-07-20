using Chimp.ViewModels;
using System.Collections.Generic;

namespace Chimp.Providers.Action
{
    abstract class ActionProvider : IActionProvider
    {
        private MainViewModel MainViewModel { get; }

        protected CardViewModel CardViewModel => CardViewModel.Get(MainViewModel);
        protected SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);
        protected CameraViewModel CameraViewModel => CameraViewModel.Get(MainViewModel);

        protected IServiceActivator ServiceActivator { get; }

        public ActionProvider(MainViewModel mainViewModel, IServiceActivator serviceActivator)
        {
            MainViewModel = mainViewModel;
            ServiceActivator = serviceActivator;
        }

        public abstract IEnumerable<IAction> GetActions();
    }
}