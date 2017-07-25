using System.Collections.Generic;

namespace Chimp.ViewModels
{
    public sealed class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            ViewModels = new Dictionary<string, ViewModel>();
        }

        private Dictionary<string, ViewModel> ViewModels { get; }

        public T Get<T>(string name)
            where T : ViewModel
        {
            return this[name] as T;
        }

        public void Set<T>(string name, T value)
            where T:ViewModel
        {
            SendPropertyChanging(string.Empty);
            if (value == null)
                ViewModels.Remove(name);
            else
                ViewModels.Add(name, value);
            SendPropertyChanged(string.Empty);
        }

        public ViewModel this[string name]
        {
            get
            {
                ViewModels.TryGetValue(name, out ViewModel value);
                return value;
            }
        }

        private StepViewModel _Step;
        public StepViewModel Step
        {
            get { return _Step; }
            set { SetProperty(ref _Step, value); }
        }

        private bool _IsWarning;
        public bool IsWarning
        {
            get { return _IsWarning; }
            set { SetProperty(ref _IsWarning, value); }
        }

        private bool _IsError;
        public bool IsError
        {
            get { return _IsError; }
            set { SetProperty(ref _IsError, value); }
        }

        private bool _IsAborted;
        public bool IsAborted
        {
            get { return _IsAborted; }
            set { SetProperty(ref _IsAborted, value); }
        }

        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set { SetProperty(ref _IsCompleted, value); }
        }

        private double _ProgressValue;
        public double ProgressValue
        {
            get { return _ProgressValue; }
            set { SetProperty(ref _ProgressValue, value); }
        }

        private bool _IsIndeterminate;
        public bool IsIndeterminate
        {
            get { return _IsIndeterminate; }
            set { SetProperty(ref _IsIndeterminate, value); }
        }
    }
}
