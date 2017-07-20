using System;
using System.Linq;

namespace Chimp.ViewModels
{
    public sealed class StepViewModel : ViewModel
    {
        private StepItemViewModel[] _Items;
        public StepItemViewModel[] Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { SetProperty(ref _SelectedIndex, value); }
        }

        public StepItemViewModel SelectedItem
        {
            get { return Items[SelectedIndex]; }
            set
            {
                SelectedIndex = value != null
					? Array.IndexOf(Items, value)
					: -1;
            }
        }

        public StepItemViewModel this[string name]
        {
            get
            {
                return Items.FirstOrDefault(s => s.Name.Equals(name, StringComparison.InvariantCulture));
            }
        }

        private bool canGoBack;
        public bool CanGoBack
        {
            get { return canGoBack; }
            set { SetProperty(ref canGoBack, value); }
        }

        private bool canContinue;
        public bool CanContinue
        {
            get { return canContinue; }
            set { SetProperty(ref canContinue, value); }
        }
    }
}
