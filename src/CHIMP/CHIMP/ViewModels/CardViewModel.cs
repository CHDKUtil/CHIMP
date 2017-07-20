using System.Collections.ObjectModel;

namespace Chimp.ViewModels
{
    sealed class CardViewModel : ViewModel//ItemsViewModel<CardItemViewModel>
    {
        public static CardViewModel Get(MainViewModel mainViewModel) => mainViewModel.Get<CardViewModel>("Card");

        private ObservableCollection<CardItemViewModel> _Items;
        public ObservableCollection<CardItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }

        private CardItemViewModel _SelectedItem;
        public CardItemViewModel SelectedItem
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
