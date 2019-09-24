namespace Chimp
{
    public interface IDialogService
    {
        void ShowErrorMessage(string message);
        bool ShowYesNoMessage(string message, string caption);
        bool ShowOkCancelMessage(string message, string caption);
        string[]? ShowOpenFileDialog(string title, string filter, bool multiselect);
    }
}
