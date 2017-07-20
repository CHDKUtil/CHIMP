namespace Chimp.ViewModels
{
    public sealed class LicensesViewModel : ViewModel
    {
        public string Title { get; set; }
        public LicensesItemViewModel[] Licenses { get; set; }

        private bool _IsAllAccepted;
        public bool IsAllAccepted
        {
            get { return _IsAllAccepted; }
            set { SetProperty(ref _IsAllAccepted, value); }
        }

        private bool _IsAllRejected;
        public bool IsAllRejected
        {
            get { return _IsAllRejected; }
            set { SetProperty(ref _IsAllRejected, value); }
        }
    }
}
