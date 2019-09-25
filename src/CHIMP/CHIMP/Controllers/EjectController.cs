﻿using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Detectors.Card;
using Net.Chdk.Watchers.Volume;
using System.Threading.Tasks;

namespace Chimp.Controllers
{
    sealed class EjectController : Controller<EjectController, EjectViewModel>
    {
        protected override bool CanSkipStep => true;
        protected override bool SkipStep => true; //Always skip, the setting indicates whether should eject

        private ICardDetector CardDetector { get; }
        private IEjectService EjectService { get; }
        private IToastService ToastService { get; }
		private IVolumeWatcher VolumeWatcher { get; }

		public EjectController(ICardDetector cardDetector, IEjectService ejectService, IToastService toastService, IVolumeWatcher volumeWatcher,
            MainViewModel mainViewModel, IStepProvider stepProvider, string stepName, ILoggerFactory loggerFactory)
            : base(mainViewModel, stepProvider, stepName, loggerFactory)
        {
            CardDetector = cardDetector;
            EjectService = ejectService;
            ToastService = toastService;
			VolumeWatcher = volumeWatcher;
        }

        protected override void EnterStep()
        {
            StepViewModel!.CanGoBack = false;

            if (ViewModel?.IsEject == true)
            {
                //ViewModel = CreateViewModel();
                ViewModel.Title = Resources.Eject_Ejecting_Text;

                VolumeWatcher.Stop();

                var card = CardViewModel?.SelectedItem?.Info;
                if (card?.DriveLetter != null)
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

        private async Task<bool> ShowToastAsync()
        {
            var displayName = CardViewModel?.SelectedItem?.DisplayName;
            return await ToastService.ShowEjectToastAsync(displayName);
        }
    }
}
