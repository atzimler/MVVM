using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATZ.MVVM.ViewModels.Utility.Connectors
{
    public class CollectionViewModelToModelConnector<VM, M> : ObservableObject<CollectionViewModelToModelConnector<VM, M>>, IVerifiable
        where VM : BaseViewModel<M>, new()
        where M : class
    {
        #region Public Delegates
        public delegate void ViewModelBinder(VM vm);
        #endregion

        #region Private Variables
        private bool _isValid;
        private ObservableCollection<M> _modelCollection;
        private ObservableCollection<VM> _viewModelCollection;
        #endregion

        #region Public Properties
        public ViewModelBinder BindViewModel { get; set; }

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

        public ObservableCollection<M> ModelCollection
        {
            get { return _modelCollection; }
            set
            {
                if (_modelCollection != value)
                {
                    UnbindModelCollection();

                    _modelCollection = value;

                    BindModelCollection();
                }
            }
        }

        public ViewModelBinder UnbindViewModel { get; set; }

        public ObservableCollection<VM> ViewModelCollection
        {
            get { return _viewModelCollection; }
            set
            {
                if (_viewModelCollection != value)
                {
                    _viewModelCollection = value;
                    ResetViewModelCollectionToModelCollection();
                }
            }
        }
        #endregion

        #region Private Methods
        private void BindModelCollection()
        {
            if (_modelCollection != null)
            {
                _modelCollection.CollectionChanged += ModelCollectionChanged;
                ResetViewModelCollectionToModelCollection();
            }
        }

        private void ClearViewModelCollection()
        {
            _viewModelCollection.ToList().ForEach(vm => DetachViewModel(vm));
            _viewModelCollection.Clear();
        }

        private VM CreateViewModelForModel(M model)
        {
            VM viewModel = new VM();
            viewModel.Model = model;
            // TODO: Replacing the viewModel.Model (above) actually executes the BindModel() below (check) and this is the only result why BindModel() is public on BaseViewModel. However,
            // the other reason could be that in this case it is similar - but results in duplicate BindModel() which could cause problems (at least performance).
            viewModel.BindModel();

            if (BindViewModel != null)
            {
                BindViewModel(viewModel);
            }

            viewModel.IsValidChanged += UpdateValidity;
            return viewModel;
        }

        private void DetachViewModel(VM viewModel)
        {
            viewModel.IsValidChanged -= UpdateValidity;
            if (UnbindViewModel != null)
            {
                UnbindViewModel(viewModel);
            }
        }

        private void ModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_modelCollection == null || _viewModelCollection == null)
            {
                return;
            }

            // TODO: This needs unit testing.
            // TODO: This and Views.Utility.CollectionConnector share similarities, maybe they can be merged.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    int insertPosition = e.NewStartingIndex;
                    foreach (M model in e.NewItems)
                    {
                        _viewModelCollection.Insert(insertPosition++, CreateViewModelForModel(model));
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    _viewModelCollection.Move(e.OldStartingIndex, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (M model in e.OldItems)
                    {
                        DetachViewModel(_viewModelCollection[e.OldStartingIndex]);
                        _viewModelCollection.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ClearViewModelCollection();
                    foreach (M model in _modelCollection)
                    {
                        _viewModelCollection.Add(CreateViewModelForModel(model));
                    }
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("CollectionViewModelToModelConnector: {0}", e.Action);
                    break;
            }

            UpdateValidity();
        }

        private void ResetViewModelCollectionToModelCollection()
        {
            ModelCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void UnbindModelCollection()
        {
            if (_modelCollection != null)
            {
                _modelCollection.CollectionChanged -= ModelCollectionChanged;
            }
        }

        private void UpdateValidity()
        {
            IsValid = _viewModelCollection == null ? true : _viewModelCollection.ToList().TrueForAll(vm => vm.IsValid);
        }
        #endregion

        #region Protected Methods
        protected virtual void OnIsValidChanged()
        {
            if (IsValidChanged != null)
            {
                IsValidChanged(this, EventArgs.Empty);
            }
            OnPropertyChanged(nameof(IsValid));
        }
        #endregion

        #region Public Methods
        public void ClearAllViewModelBindings()
        {
            if (_viewModelCollection != null)
            {
                _viewModelCollection.ToList().ForEach(vm => DetachViewModel(vm));
            }
        }

        public void Sort(Comparison<M> comparison)
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int index = 0; index < _modelCollection.Count - 1; ++index)
                {
                    if (comparison(_modelCollection[index], _modelCollection[index + 1]) > 0)
                    {
                        _modelCollection.Move(index + 1, index);
                        swapped = true;
                    }
                }
            } while (swapped);
        }

        public void UpdateValidity(object sender, EventArgs e)
        {
            UpdateValidity();
        }
        #endregion
    }
}
