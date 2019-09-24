using Chimp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Chimp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IServiceProvider ServiceProvider { get; }
        private IStepProvider StepProvider { get; }
        private IPageContainer PageContainer { get; }
        private IControllerContainer ControllerContainer { get; }
        private IDialogService DialogService { get; }
        private ILogger Logger => ServiceProvider.GetService<ILogger<MainWindow>>();
        private MainViewModel ViewModel => ServiceProvider.GetService<MainViewModel>();

        public MainWindow(IServiceProvider serviceProvider, IStepProvider stepProvider, IPageContainer pageContainer, IControllerContainer controllerContainer, IDialogService dialogService)
        {
            ServiceProvider = serviceProvider;
            StepProvider = stepProvider;
            PageContainer = pageContainer;
            ControllerContainer = controllerContainer;
            DialogService = dialogService;

            ViewModel.Set("Eject", CreateEjectViewModel());

            ViewModel.Step = CreateSteps();
            ViewModel.Step.PropertyChanging += Step_PropertyChanging;
            ViewModel.Step.PropertyChanged += Step_PropertyChanged;

            InitializeComponent();
            DataContext = ViewModel;
        }

        private EjectViewModel CreateEjectViewModel()
        {
            return new EjectViewModel
            {
                IsEject = !StepProvider.IsSkip("Eject"),
            };
        }

        private StepViewModel CreateSteps()
        {
            var steps = StepProvider.GetSteps();
            var items = steps.Select(CreateStep).ToArray();
            return new StepViewModel
            {
                Items = items,
            };
        }

        private StepItemViewModel CreateStep(string name)
        {
            return new StepItemViewModel
            {
                Name = name,
                IsVisible = !StepProvider.IsHidden(name)
            };
        }

        private async void Step_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(StepViewModel.SelectedIndex):
                    await OnSelectedIndexChangingAsync();
                    break;
                default:
                    break;
            }
        }

        private async void Step_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(StepViewModel.SelectedIndex):
                    await OnSelectedIndexChangedAsync();
                    break;
                default:
                    break;
            }
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await OnSelectedIndexChangedAsync();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (ViewModel.IsAborted)
                return;

            if (ViewModel.IsCompleted)
            {
                return;
            }

            Logger.LogTrace("Closing");

            if (ShowClosingMessage())
            {
                if (!ViewModel.IsCompleted)
                    ViewModel.IsAborted = true;
                ViewModel.Step!.SelectedIndex = ViewModel.Step.Items!.Length - 1;
            }

            e.Cancel = true;
        }

        protected override async void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            await OnSelectedIndexChangingAsync();
            ControllerContainer.Dispose();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogTrace("Clicked GoBack");
            GoBack();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogTrace("Clicked Continue");
            Continue();
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogTrace("Clicked Finish");
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogTrace("Clicked Close");
            Close();
        }

        private void GoBack()
        {
            int index = ViewModel.Step!.SelectedIndex;
            while (--index >= 0 && ViewModel.Step.Items![index].IsSkipped) ;
            if (index >= 0)
                ViewModel.Step.SelectedIndex = index;
        }

        private void Continue()
        {
            var index = ViewModel.Step!.SelectedIndex;
            while (++index < ViewModel.Step.Items!.Length && ViewModel.Step.Items[index].IsSkipped) ;
            if (index < ViewModel.Step.Items.Length)
                ViewModel.Step.SelectedIndex = index;
        }

        private async Task OnSelectedIndexChangingAsync()
        {
            int index = ViewModel.Step!.SelectedIndex;
            var length = ViewModel.Step.Items!.Length;
            if (index == length)
            {
                return;
            }
            var item = ViewModel.Step.Items[index];
            var name = item.Name!;
            Logger.LogTrace("Leaving {0}", name);
            item.IsSelected = false;
            var controller = await ControllerContainer.GetControllerAsync(name);
            await controller.LeaveStepAsync();
        }

        private async Task OnSelectedIndexChangedAsync()
        {
            int index = ViewModel.Step!.SelectedIndex;
            var length = ViewModel.Step.Items!.Length;
            if (index == length)
            {
                Close();
                return;
            }

            var item = ViewModel.Step.Items[index];
            var name = item.Name!;
            Logger.LogTrace("Entering {0}", name);
            item.IsSelected = true;
            ViewModel.Step.CanGoBack = CanGoBack(index);
            var page = PageContainer.GetPage(name);
            frame.Navigate(page);
            var controller = await ControllerContainer.GetControllerAsync(name);
            await controller.EnterStepAsync();
        }

        private bool CanGoBack(int index)
        {
            for (int i = index - 1; i >= 0; i--)
                if (!ViewModel.Step!.Items![i].IsSkipped)
                    return true;
            return false;
        }

        private bool ShowClosingMessage()
        {
            var message = Properties.Resources.MessageBox_Abort_Text;
            var caption = Properties.Resources._Title;
            return DialogService.ShowYesNoMessage(message, caption);
        }
    }
}
