using System.ComponentModel;

namespace ATZ.MVVM.ViewModels.Utility
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected void Set<T>(string propertyName, ref T propertyStorage, T newValue)
        {
            if (propertyStorage.Equals(newValue)) return;

            propertyStorage = newValue;
            OnPropertyChanged(propertyName);
        }

        protected SuspendPropertyChangedEvent SuspendPropertyChangedEvent(PropertyChangedEventHandler eventHandler)
        {
            PropertyChanged -= eventHandler;
            return new SuspendPropertyChangedEvent(() => PropertyChanged += eventHandler);
        }
    }
}
