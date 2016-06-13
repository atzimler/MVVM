using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        protected void Set<T>(ref T propertyStorage, T newValue, IEnumerable<string> additionalPropertiesChanged = null, [CallerMemberName] string propertyName = null)
        {
            Set(propertyName, ref propertyStorage, newValue);
            additionalPropertiesChanged?.ToList().ForEach(OnPropertyChanged);
        }

        protected SuspendPropertyChangedEvent SuspendPropertyChangedEvent(PropertyChangedEventHandler eventHandler)
        {
            PropertyChanged -= eventHandler;
            return new SuspendPropertyChangedEvent(() => PropertyChanged += eventHandler);
        }
    }
}
