using Chimp.ViewModels;
using System.Windows;
using Microsoft.Win32;

namespace Chimp.Services
{
    sealed class DialogService : IDialogService
    {
        private MainViewModel MainViewModel { get; }

        public DialogService(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        public void ShowErrorMessage(string message)
        {
            ShowErrorMessage(message, MessageBoxButton.OK, MessageBoxResult.OK);
        }

        public bool ShowYesNoMessage(string message, string caption)
        {
            return ShowExclamationMessage(message, caption, MessageBoxButton.YesNo, MessageBoxResult.Yes);
        }

        public bool ShowOkCancelMessage(string message, string caption)
        {
            return ShowExclamationMessage(message, caption, MessageBoxButton.OKCancel, MessageBoxResult.OK);
        }

        public string[]? ShowOpenFileDialog(string title, string filter, bool multiselect)
        {
            var dlg = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = multiselect,
            };
            if (dlg.ShowDialog() != true)
                return null;
            return dlg.FileNames;
        }

        private bool ShowErrorMessage(string message, MessageBoxButton button, MessageBoxResult defaultResult)
        {
            var wasError = MainViewModel.IsError;
            MainViewModel.IsError = true;
            var result = MessageBox.Show(message, Properties.Resources.MessageBox_Error_Caption, button, MessageBoxImage.Error);
            MainViewModel.IsError = wasError;
            return result == defaultResult;
        }

        private bool ShowExclamationMessage(string message, string caption, MessageBoxButton button, MessageBoxResult defaultResult)
        {
            var wasWarning = MainViewModel.IsWarning;
            MainViewModel.IsWarning = true;
            var result = MessageBox.Show(message, caption, button, MessageBoxImage.Exclamation);
            MainViewModel.IsWarning = wasWarning;
            return result == defaultResult;
        }
    }
}
