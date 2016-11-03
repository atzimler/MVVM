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
            // TODO: This needs to be figured out why I have the virtual call here (probably do a test package without the virtual function and see what is hapening when I refactor the dependent functions)
            InitializeComponent();
        }

        // TODO: For trying it out.
        //protected virtual void InitializeComponent()
        protected void InitializeComponent()
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
