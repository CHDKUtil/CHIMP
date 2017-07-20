using Net.Chdk.Model.Software;

namespace Chimp.ViewModels
{
    sealed class SoftwareViewModel : ItemsViewModel<SoftwareItemViewModel>
    {
        public static SoftwareViewModel Get(MainViewModel mainViewModel) => mainViewModel.Get<SoftwareViewModel>("Software");

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set { SetProperty(ref _IsCompleted, value); }
        }
    }
}
