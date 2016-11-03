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

                if (_model == null)
                {
                    return;
                }

                BindModel();
                UpdateValidity(this, EventArgs.Empty);
            }
        }

        protected BaseViewModel()
        {
            // ReSharper disable once VirtualMemberCallInConstructor => Because Model Binding can occur as soon as object is constructed at this level, the components have to be
            // initialized when passing this point. However, top level constructors could initialize a component that is already too late, because lower level constructor tries
            // to set the Model on the ViewModel. As a result, NullReferenceExceptions might occur. We are following the solution used in the WPF code and InitializeComponent is
            // the place to initialize those objects that might be accessed during View/ViewModel interactions.
            InitializeComponent();
        }

        /// <summary>
        /// Initialize components of the ViewModel.
        /// </summary>
        /// <remarks>
        /// Because Model Binding can occur as soon as object is constructed at this level, the components have to be
        /// initialized when passing this point. However, top level constructors could initialize a component that is already too late, because lower level constructor tries
        /// to set the Model on the ViewModel. As a result, NullReferenceExceptions might occur. We are following the solution used in the WPF code and InitializeComponent is
        /// the place to initialize those objects that might be accessed during View/ViewModel interactions.
        /// To avoid problems, overriden InitializeComponent methods should call base InitializeComponent method, usually as a first operation, but might be exceptions.
        /// </remarks>
        // ReSharper disable once VirtualMemberNeverOverridden.Global => Part of Public API
        protected virtual void InitializeComponent()
        {
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global => Part of Public API
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
