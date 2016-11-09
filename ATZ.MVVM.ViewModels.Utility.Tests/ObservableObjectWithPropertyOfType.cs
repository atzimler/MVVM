namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    public class ObservableObjectWithPropertyOfType<T> : ObservableObject
    {
        private T _property;

        public T Property
        {
            get { return _property; }
            set { Set(ref _property, value); }
        }
    }
}
