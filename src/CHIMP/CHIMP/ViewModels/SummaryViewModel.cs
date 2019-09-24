using Chimp.Model;

namespace Chimp.ViewModels
{
    public sealed class SummaryViewModel : ViewModel
    {
        public static SummaryViewModel? Get(MainViewModel mainViewModel) => mainViewModel.Get<SummaryViewModel>("Summary");

        public string? Title { get; set; }
        public string? Message { get; set; }
        public Tip[]? Tips { get; set; }
        public string? UserManualUrl { get; set; }
        public string? CameraNotesFileName { get; set; }

        private bool _IsOpenUserManual;
        public bool IsOpenUserManual
        {
            get { return _IsOpenUserManual; }
            set { SetProperty(ref _IsOpenUserManual, value); }
        }

        private bool _IsShowCameraNotes;
        public bool IsShowCameraNotes
        {
            get { return _IsShowCameraNotes; }
            set { SetProperty(ref _IsShowCameraNotes, value); }
        }

    }
}
