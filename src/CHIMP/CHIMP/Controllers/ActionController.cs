using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Linq;

namespace Chimp.Controllers
{
    sealed class ActionController : Controller<ActionController, ActionViewModel>
    {
        protected override bool CanSkipStep => IsCanSkipStep(ViewModel);
        protected override bool SkipStep => IsSkipStep(ViewModel);

        private static bool IsCanSkipStep(ActionViewModel viewModel)
        {
            return viewModel.SelectedItem != null;
        }

        private bool IsSkipStep(ActionViewModel viewModel)
        {
            return viewModel.Items.Length == 1 || base.SkipStep;
        }

        private IActionProvider ActionProvider { get; }

        public ActionController(IActionProvider actionProvider, MainViewModel mainViewModel, IStepProvider stepProvider, string stepName, ILoggerFactory loggerFactory)
            : base(mainViewModel, stepProvider, stepName, loggerFactory)
        {
            ActionProvider = actionProvider;
        }

        protected override void Initialize()
        {
			base.Initialize();
			Subscribe();
        }

        public override void Dispose()
        {
            base.Dispose();
            Unsubscribe();
        }

        protected override void EnterStep()
        {
            if (ViewModel == null)
                ViewModel = CreateViewModel();
            Subscribe2();
            UpdateCanContinue();
            UpdateIsPaused();
        }

        protected override void LeaveStep()
        {
            Unsubscribe2();
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            CameraViewModel.PropertyChanged += CameraViewModel_PropertyChanged;
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            CameraViewModel.PropertyChanged -= CameraViewModel_PropertyChanged;
        }

        protected override void Card_SelectedItemChanged()
        {
            ViewModel = null;
        }

        protected override void Software_SelectedItemChanged()
        {
            ViewModel = null;
        }

        private void CameraViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CameraViewModel.SelectedItem):
                    Camera_SelectedItemChanged();
                    break;
                default:
                    break;
            }
        }

        private void Camera_SelectedItemChanged()
        {
            ViewModel = null;
        }

        private void Subscribe2()
        {
            ViewModel.PropertyChanged += Action_PropertyChanged;
        }

        private void Unsubscribe2()
        {
            ViewModel.PropertyChanged -= Action_PropertyChanged;
        }

        private void UpdateCanContinue()
        {
            StepViewModel.CanContinue = ViewModel?.SelectedItem != null;
        }

        private void UpdateIsPaused()
        {
            MainViewModel.IsWarning = ViewModel?.SelectedItem == null;
        }

        private ActionViewModel CreateViewModel()
        {
            var actions = ActionProvider.GetActions();
            var items = actions
                .Select(CreateAction)
                .ToArray();

            var viewModel = new ActionViewModel
            {
                Items = items,
                SelectedItem = items.FirstOrDefault(item => item.Action.IsDefault),
            };

            var isSkip = IsCanSkipStep(viewModel) && IsSkipStep(viewModel);
            viewModel.IsSelect = !isSkip;
            if (items.Length == 0)
                viewModel.Title = Resources.Action_Unsupported_Text;
            else if (!isSkip)
                viewModel.Title = Resources.Action_Select_Text;

            return viewModel;
        }

        private ActionItemViewModel CreateAction(IAction action)
        {
            return new ActionItemViewModel
            {
                Action = action,
                DisplayName = action.DisplayName,
            };
        }

        private void Action_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ActionViewModel.SelectedItem):
                    Logger.LogObject(LogLevel.Information, "Selected {0}", ViewModel.SelectedItem.DisplayName);
                    UpdateCanContinue();
                    UpdateIsPaused();
                    break;
                default:
                    break;
            }
        }
    }
}
