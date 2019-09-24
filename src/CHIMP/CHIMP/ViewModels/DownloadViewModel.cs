using Net.Chdk.Model.Software;

namespace Chimp.ViewModels
{
    public sealed class DownloadViewModel : ViewModel
    {
        public static DownloadViewModel? Get(MainViewModel mainViewModel) => mainViewModel.Get<DownloadViewModel>("Download");

        private string? _Title;
        public string? Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        private string? _FileName;
        public string? FileName
        {
            get { return _FileName; }
            set { SetProperty(ref _FileName, value); }
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

        private bool _IsUpToDate;
        public bool IsUpToDate
        {
            get { return _IsUpToDate; }
            set { SetProperty(ref _IsUpToDate, value); }
        }

        private SoftwareInfo? _Software;
        public SoftwareInfo? Software
        {
            get { return _Software; }
            set { SetProperty(ref _Software, value); }
        }

        private string[]? _Paths;
        public string[]? Paths
        {
            get { return _Paths; }
            set { SetProperty(ref _Paths, value); }
        }
    }
}
