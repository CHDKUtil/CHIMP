using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Detectors.Card;
using Net.Chdk.Watchers.Volume;
using System.Threading.Tasks;

namespace Chimp.Controllers
{
    sealed class EjectController : Controller<EjectController, EjectViewModel>
    {
        protected override string StepName => "Eject";
        protected override bool CanSkipStep => true;
        protected override bool SkipStep => true; //Always skip, the setting indicates whether should eject

        private ICardDetector CardDetector { get; }
        private IEjectService EjectService { get; }
        private IToastService ToastService { get; }
		private IVolumeWatcher VolumeWatcher { get; }

		public EjectController(ICardDetector cardDetector, IEjectService ejectService, IToastService toastService, IVolumeWatcher volumeWatcher, MainViewModel mainViewModel, ILoggerFactory loggerFactory)
            : base(mainViewModel, loggerFactory)
        {
            CardDetector = cardDetector;
            EjectService = ejectService;
            ToastService = toastService;
			VolumeWatcher = volumeWatcher;
        }

        protected override void EnterStep()
        {
            StepViewModel.CanGoBack = false;

            if (ViewModel.IsEject)
            {
                //ViewModel = CreateViewModel();
                ViewModel.Title = Resources.Eject_Ejecting_Text;

                VolumeWatcher.Stop();

                var card = CardViewModel.SelectedItem?.Info;
                if (card != null)
                {
                    EjectService.Eject(card.DriveLetter);
                    var card2 = CardDetector.GetCard(card.DriveLetter);
                    ViewModel.IsCompleted = card2 == null;
                }
            }
        }

        public override async Task LeaveStepAsync()
        {
            await base.LeaveStepAsync();
            if (ViewModel?.IsCompleted == true && ToastService.IsAvailable)
                await ShowToastAsync();
            if (InstallViewModel?.IsCompleted == true)
                MainViewModel.IsCompleted = true;
        }

        private EjectViewModel CreateViewModel()
        {
            return new EjectViewModel
            {
                Title = Resources.Eject_Ejecting_Text,
            };
        }

        private async Task<bool> ShowToastAsync()
        {
            var displayName = CardViewModel?.SelectedItem.DisplayName;
            return await ToastService.ShowEjectToastAsync(displayName);
        }
    }
}
