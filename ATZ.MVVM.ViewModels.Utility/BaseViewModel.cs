using System;


namespace ATZ.MVVM.ViewModels.Utility
{
    public abstract class BaseViewModel<T> : ObservableObject, IVerifiable
        where T : class
    {
        private bool _isValid;
        private T _model;

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (_isValid == value)
                {
                    return;
                }

                _isValid = value;
                OnIsValidChanged();
            }
        }

        public event EventHandler IsValidChanged;
  
        public T Model
        {
            get { return _model; }
            set
            {
                if (_model == value)
                {
                    return;
                }

                if (_model != null)
                {
                    UnbindModel();
                }
                _model = value;
                if (_model != null)
                {
                    BindModel();
                    UpdateValidity(this, EventArgs.Empty);
                }
            }
        }

        public BaseViewModel()
        {
            InitializeComponent();
        }

        protected virtual void InitializeComponent()
        {
        }

        protected virtual void OnIsValidChanged()
        {
            IsValidChanged?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(IsValid));
        }

        protected abstract void BindModel();
        protected abstract void UnbindModel();

        public abstract void UpdateValidity(object sender, EventArgs e);
    }
}
