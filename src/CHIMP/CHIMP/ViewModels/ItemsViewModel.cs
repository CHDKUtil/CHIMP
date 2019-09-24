namespace Chimp.ViewModels
{
    abstract class ItemsViewModel<TItemViewModel>: ViewModel
        where TItemViewModel : class
    {
        private TItemViewModel[]? _Items;
        public TItemViewModel[]? Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }

        private TItemViewModel? _SelectedItem;
        public TItemViewModel? SelectedItem
        {
            get { return _SelectedItem; }
            set { SetProperty(ref _SelectedItem, value); }
        }

        private bool _IsSelect;
        public bool IsSelect
        {
            get { return _IsSelect; }
            set { SetProperty(ref _IsSelect, value); }
        }
    }
}
