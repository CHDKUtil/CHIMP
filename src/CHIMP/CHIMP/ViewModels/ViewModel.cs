using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chimp.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            SendPropertyChanging(propertyName);
            storage = value;
            SendPropertyChanged(propertyName);
            return true;
        }

        protected void SendPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected void SendPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
