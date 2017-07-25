using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chimp.Controllers
{
    abstract class Controller<T> : IController
        where T : Controller<T>
    {
        protected CancellationTokenSource cts;

        protected Controller(MainViewModel mainViewModel, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger<T>();
            MainViewModel = mainViewModel;
        }

        public virtual void Dispose()
        {
			if (cts != null)
				cts.Dispose();
        }

        public async Task InitializeAsync()
        {
            await Task.Run(() => Initialize())
                .ContinueWith(OnInitialized, TaskScheduler.FromCurrentSynchronizationContext());
        }

		protected virtual void Initialize()
		{
			StepViewModel.CanContinue = false;
		}

		private void OnInitialized(Task task)
        {
            if (task.Exception != null)
                throw task.Exception;
        }

        public virtual async Task EnterStepAsync()
        {
            StepViewModel.CanContinue = false;

            await Task.Run(() => EnterStep())
                .ContinueWith(OnStepEntered, TaskScheduler.FromCurrentSynchronizationContext());
        }

		protected virtual void EnterStep()
		{
		}

		private void OnStepEntered(Task task)
        {
            if (task.Exception != null)
                throw task.Exception;

            TrySkipStep();
        }

        public virtual async Task LeaveStepAsync()
        {
            await Task.Run(() => LeaveStep())
				.ContinueWith(OnStepLeft, TaskScheduler.FromCurrentSynchronizationContext());
        }

		protected virtual void LeaveStep()
		{
			cts?.Cancel();
		}

		protected virtual void OnStepLeft(Task task)
		{
			if (task.Exception != null)
				throw task.Exception;
		}

		protected void TrySkipStep()
        {
            if (CanSkipStep && SkipStep)
            {
                Logger.LogTrace("Skipping {0}", StepName);
                StepViewModel.SelectedItem.IsSkipped = true;
                StepViewModel.SelectedIndex++;
            }
        }

        protected abstract string StepName { get; }

        protected abstract bool CanSkipStep { get; }

        protected virtual bool SkipStep =>
            MainViewModel.Settings.SkipSteps?.Contains(StepName, StringComparer.OrdinalIgnoreCase) == true;

        protected ILoggerFactory LoggerFactory { get; }

        protected ILogger Logger { get; }

        protected MainViewModel MainViewModel { get; }

        protected StepViewModel StepViewModel => MainViewModel.Step;
        protected CardViewModel CardViewModel => CardViewModel.Get(MainViewModel);
        protected SoftwareViewModel SoftwareViewModel => SoftwareViewModel.Get(MainViewModel);
        protected CameraViewModel CameraViewModel => CameraViewModel.Get(MainViewModel);
        protected ActionViewModel ActionViewModel => ActionViewModel.Get(MainViewModel);
        protected DownloadViewModel DownloadViewModel => DownloadViewModel.Get(MainViewModel);
        protected InstallViewModel InstallViewModel => InstallViewModel.Get(MainViewModel);
        protected EjectViewModel EjectViewModel => EjectViewModel.Get(MainViewModel);
    }

    abstract class Controller<TController, TViewModel> : Controller<TController>
        where TController : Controller<TController, TViewModel>
        where TViewModel : ViewModel
    {
        public Controller(MainViewModel mainViewModel, ILoggerFactory loggerFactory)
            : base(mainViewModel, loggerFactory)
        {
        }

        protected virtual void Subscribe()
        {
            if (CardViewModel != null)
                CardViewModel.PropertyChanged += Card_PropertyChanged;
            if (SoftwareViewModel != null)
                SoftwareViewModel.PropertyChanged += Software_PropertyChanged;
        }

        protected virtual void Unsubscribe()
        {
            if (CardViewModel != null)
                CardViewModel.PropertyChanged -= Card_PropertyChanged;
            if (SoftwareViewModel != null)
                SoftwareViewModel.PropertyChanged -= Software_PropertyChanged;
        }

        private void Card_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CardViewModel.SelectedItem):
                    Card_SelectedItemChanged();
                    break;
                default:
                    break;
            }
        }

        private void Software_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SoftwareViewModel.SelectedItem):
                    Software_SelectedItemChanged();
                    break;
                default:
                    break;
            }
        }

        protected virtual void Card_SelectedItemChanged()
        {
        }

        protected virtual void Software_SelectedItemChanged()
        {
        }

        protected TViewModel ViewModel
        {
            get { return MainViewModel.Get<TViewModel>(StepName); }
            set { MainViewModel.Set(StepName, value); }
        }
    }
}
