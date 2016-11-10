using System;


namespace ATZ.MVVM.ViewModels.Utility
{
    /// <summary>
    /// Base view model class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseViewModel<T> : ObservableObject, IViewModel<T>
        where T : class
    {
        private bool _isValid;
        private T _model;

        /// <summary>
        /// The validity of the object.
        /// </summary>
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

        /// <summary>
        /// The validity of the object has changed.
        /// </summary>
        public event EventHandler IsValidChanged;
  
        /// <summary>
        /// The model object associated with the ViewModel.
        /// </summary>
        public T Model
        {
            get { return _model; }
            // ReSharper disable once MemberCanBeProtected.Global => Part of public API.
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

        /// <summary>
        /// Constructor.
        /// </summary>
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

        /// <summary>
        /// Fire IsValidChanged event and property change notification event for the IsValid property.
        /// </summary>
        // ReSharper disable once VirtualMemberNeverOverridden.Global => Part of Public API
        protected virtual void OnIsValidChanged()
        {
            IsValidChanged?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(IsValid));
        }

        /// <summary>
        /// Bind the Model to the ViewModel.
        /// </summary>
        protected abstract void BindModel();

        /// <summary>
        /// Unbind the Model from the ViewModel.
        /// </summary>
        protected abstract void UnbindModel();

        /// <summary>
        /// Get the value of the Model property in a covariant type implementation way.
        /// </summary>
        /// <returns>The value of the Model property.</returns>
        public T GetModel()
        {
            return Model;
        }

        /// <summary>
        /// Set the value of the Model proeprty in a contravariant type implementation way.
        /// </summary>
        /// <param name="model">The new value of the Model property.</param>
        public void SetModel(T model)
        {
            Model = model;
        }

        /// <summary>
        /// Reevaluation of the object validity requested.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        public abstract void UpdateValidity(object sender, EventArgs e);
    }
}
