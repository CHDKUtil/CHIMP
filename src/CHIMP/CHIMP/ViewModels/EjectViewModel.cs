namespace Chimp.ViewModels
{
    sealed class EjectViewModel : ViewModel
    {
        public static EjectViewModel? Get(MainViewModel mainViewModel) => mainViewModel.Get<EjectViewModel>("Eject");

        public string? Title { get; set; }

        private bool _IsEject;
        public bool IsEject
        {
            get { return _IsEject; }
            set { SetProperty(ref _IsEject, value); }
        }

        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set { SetProperty(ref _IsCompleted, value); }
        }
    }
}
