using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Utility
{
    public abstract class BaseViewModel<T> : ObservableObject<BaseViewModel<T>>, IVerifiable
        where T : class
    {
        #region Private Variables
        private bool _isValid;
        private T _model;
        #endregion

        #region Public Properties
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnIsValidChanged();
                }
            }
        }

        public event EventHandler IsValidChanged;
  
        public T Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
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
        }
        #endregion

        #region Constructors
        public BaseViewModel()
        {
            InitializeComponent();
        }
        #endregion

        #region Protected Methods
        protected virtual void InitializeComponent()
        {
        }

        protected virtual void OnIsValidChanged()
        {
            if (IsValidChanged != null)
            {
                IsValidChanged(this, EventArgs.Empty);
            }
            OnPropertyChanged(nameof(IsValid));
        }
        
        protected abstract void UnbindModel();
        #endregion

        #region Public Methods
        public abstract void BindModel();
        public abstract void UpdateValidity(object sender, EventArgs e);
        #endregion
    }
}
