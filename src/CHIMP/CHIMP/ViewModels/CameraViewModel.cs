using Net.Chdk.Model.Camera;

namespace Chimp.ViewModels
{
    sealed class CameraViewModel : ItemsViewModel<CameraItemViewModel>
    {
        public static CameraViewModel? Get(MainViewModel mainViewModel) => mainViewModel.Get<CameraViewModel>("Camera");

        private string? _Error;
        public string? Error
        {
            get { return _Error; }
            set { SetProperty(ref _Error, value); }
        }

        private CameraInfo? _Info;
        public CameraInfo? Info
        {
            get { return _Info; }
            set { SetProperty(ref _Info, value); }
        }

        public string? CardType { get; set; }
    }
}
