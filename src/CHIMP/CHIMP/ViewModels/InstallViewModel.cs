namespace Chimp.ViewModels
{
    public sealed class InstallViewModel : ViewModel
    {
        public static InstallViewModel Get(MainViewModel mainViewModel) => mainViewModel.Get<InstallViewModel>("Install");

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        private int _ProgressMaximum;
        public int ProgressMaximum
        {
            get { return _ProgressMaximum; }
            set { SetProperty(ref _ProgressMaximum, value); }
        }

        private int _ProgressValue;
        public int ProgressValue
        {
            get { return _ProgressValue; }
            set { SetProperty(ref _ProgressValue, value); }
        }

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { SetProperty(ref _FileName, value); }
        }

        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set { SetProperty(ref _IsCompleted, value); }
        }

        private bool _IsAborted;
        public bool IsAborted
        {
            get { return _IsAborted; }
            set { SetProperty(ref _IsAborted, value); }
        }
    }
}
