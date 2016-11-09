using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ATZ.MVVM.ViewModels.Utility
{
    /// <summary>
    /// Abstract base class to provide INotifyPropertyChanged implementation.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        private static bool SafeEqualityCheck<T>(ref T propertyStorage, T value)
        {
            return typeof(T).IsValueType ? propertyStorage.Equals(value) : ReferenceEquals(propertyStorage, value);
        }

        /// <summary>
        /// Fire PropertyChanged event if there is an attached one.
        /// </summary>
        /// <param name="propertyName">The name of the property that has been changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ReSharper disable once MemberCanBePrivate.Global => Part of the public API
        /// <summary>
        /// Set a property with given property name and storage to the new value.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property. Use nameof() to enlist the help of the compiler for making sure that the name stays connected to the property.</param>
        /// <param name="propertyStorage">The backstorage of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        protected void Set<T>(string propertyName, ref T propertyStorage, T newValue)
        {
            if (SafeEqualityCheck(ref propertyStorage, newValue))
            {
                return;
            }

            propertyStorage = newValue;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Set a property with given property name and storage to the new value, with signalling additional properties as being changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyStorage">The backstorage of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        /// <param name="additionalPropertiesChanged">The additional properties that are affected by the change.</param>
        /// <param name="propertyName">The name of the property changed. If omitted then the calling property is the property.</param>
        protected void Set<T>(ref T propertyStorage, T newValue, IEnumerable<string> additionalPropertiesChanged = null, [CallerMemberName] string propertyName = null)
        {
            Set(propertyName, ref propertyStorage, newValue);
            additionalPropertiesChanged?.ToList().ForEach(OnPropertyChanged);
        }

        /// <summary>
        /// Create a using object that detaches an event handler from this object and reattach it when the object is disposed.
        /// </summary>
        /// <param name="eventHandler">The event handler to detach and reattach.</param>
        /// <returns>The object that when used in a using block is detaching and reattaching the event handler.</returns>
        protected SuspendPropertyChangedEvent SuspendPropertyChangedEvent(PropertyChangedEventHandler eventHandler)
        {
            PropertyChanged -= eventHandler;
            return new SuspendPropertyChangedEvent(() => PropertyChanged += eventHandler);
        }
    }
}
