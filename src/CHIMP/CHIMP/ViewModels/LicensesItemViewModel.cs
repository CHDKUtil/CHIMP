namespace Chimp.ViewModels
{
    public sealed class LicensesItemViewModel : ViewModel
    {
        public string Product { get; set; }

        public string[] Contents { get; set; }

        private bool _IsAccepted;
        public bool IsAccepted
        {
            get { return _IsAccepted; }
            set { SetProperty(ref _IsAccepted, value); }
        }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set { SetProperty(ref _IsExpanded, value); }
        }
    }
}
