using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ATZ.MVVM.ViewModels.Utility
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ReSharper disable once MemberCanBePrivate.Global => Part of the API
        protected void Set<T>(string propertyName, ref T propertyStorage, T newValue)
        {
            if (propertyStorage.Equals(newValue)) return;

            propertyStorage = newValue;
            OnPropertyChanged(propertyName);
        }

        protected void Set<T>(ref T propertyStorage, T newValue, [CallerMemberName] string propertyName = null)
        {
            Set(propertyName, ref propertyStorage, newValue);
        }

        protected SuspendPropertyChangedEvent SuspendPropertyChangedEvent(PropertyChangedEventHandler eventHandler)
        {
            PropertyChanged -= eventHandler;
            return new SuspendPropertyChangedEvent(() => PropertyChanged += eventHandler);
        }
    }
}
