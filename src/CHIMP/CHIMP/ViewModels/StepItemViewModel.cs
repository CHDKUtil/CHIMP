namespace Chimp.ViewModels
{
    public sealed class StepItemViewModel : ViewModel
    {
        public string Name { get; set; }

        private bool _IsVisible;
        public bool IsVisible
        {
            get { return _IsVisible; }
            set { SetProperty(ref _IsVisible, value); }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }

        private bool _IsSkipped;
        public bool IsSkipped
        {
            get { return _IsSkipped; }
            set { SetProperty(ref _IsSkipped, value); }
        }
    }
}
