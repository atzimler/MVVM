using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ATZ.MVVM.ViewModels.Utility.Connectors
{
    public class CollectionViewModelToModelConnector<TViewModel, TModel> : ObservableObject, IVerifiable
        where TViewModel : BaseViewModel<TModel>, new()
        where TModel : class
    {
        public delegate void ViewModelBinder(TViewModel vm);

        private bool _isValid;
        private ObservableCollection<TModel> _modelCollection;
        private ObservableCollection<TViewModel> _viewModelCollection;

        public ViewModelBinder BindViewModel { get; set; }

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

        public ObservableCollection<TModel> ModelCollection
        {
            get { return _modelCollection; }
            set
            {
                if (_modelCollection == value)
                {
                    return;
                }

                UnbindModelCollection();

                _modelCollection = value;

                BindModelCollection();
            }
        }

        public ViewModelBinder UnbindViewModel { get; set; }

        public ObservableCollection<TViewModel> ViewModelCollection
        {
            get { return _viewModelCollection; }
            set
            {
                if (_viewModelCollection == value)
                {
                    return;
                }

                _viewModelCollection = value;
                ResetViewModelCollectionToModelCollection();
            }
        }

        private void BindModelCollection()
        {
            if (_modelCollection == null)
            {
                return;
            }

            _modelCollection.CollectionChanged += ModelCollectionChanged;
            ResetViewModelCollectionToModelCollection();
        }

        private void ClearViewModelCollection()
        {
            _viewModelCollection.ToList().ForEach(DetachViewModel);
            _viewModelCollection.Clear();
        }

        private TViewModel CreateViewModelForModel(TModel model)
        {
            var viewModel = new TViewModel { Model = model };
            // TODO: Replacing the viewModel.Model (above) actually executes the BindModel() below (check) and this is the only result why BindModel() is public on BaseViewModel. However,
            // the other reason could be that in this case it is similar - but results in duplicate BindModel() which could cause problems (at least performance).
            viewModel.BindModel();

            BindViewModel?.Invoke(viewModel);

            viewModel.IsValidChanged += UpdateValidity;
            return viewModel;
        }

        private void DetachViewModel(TViewModel viewModel)
        {
            viewModel.IsValidChanged -= UpdateValidity;
            if (UnbindViewModel != null)
            {
                UnbindViewModel(viewModel);
            }
        }

        private void Add(NotifyCollectionChangedEventArgs e)
        {
            var insertPosition = e.NewStartingIndex;
            foreach (TModel model in e.NewItems)
            {
                _viewModelCollection.Insert(insertPosition++, CreateViewModelForModel(model));
            }
        }
        private void Move(NotifyCollectionChangedEventArgs e)
        {
            _viewModelCollection.Move(e.OldStartingIndex, e.NewStartingIndex);
        }

        private void Remove(NotifyCollectionChangedEventArgs e)
        {
            foreach (TModel model in e.OldItems)
            {
                DetachViewModel(_viewModelCollection[e.OldStartingIndex]);
                _viewModelCollection.RemoveAt(e.OldStartingIndex);
            }
        }

        private void Reset(NotifyCollectionChangedEventArgs e)
        {
            ClearViewModelCollection();
            foreach (TModel model in _modelCollection)
            {
                _viewModelCollection.Add(CreateViewModelForModel(model));
            }
        }

        private void Replace(NotifyCollectionChangedEventArgs e)
        {
            DetachViewModel(_viewModelCollection[e.OldStartingIndex]);
            _viewModelCollection[e.OldStartingIndex] =
                CreateViewModelForModel(_modelCollection[e.OldStartingIndex]);
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
                    Add(e);
                    break;
                case NotifyCollectionChangedAction.Move:
                    Move(e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Remove(e);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reset(e);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Replace(e);
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

        protected virtual void OnIsValidChanged()
        {
            if (IsValidChanged != null)
            {
                IsValidChanged(this, EventArgs.Empty);
            }
            OnPropertyChanged(nameof(IsValid));
        }

        public void AddModelWithViewModel(TModel model, TViewModel viewModel)
        {
            if (_modelCollection == null || _viewModelCollection == null)
            {
                return;
            }

            _modelCollection.CollectionChanged -= ModelCollectionChanged;

            _modelCollection.Add(model);
            _viewModelCollection.Add(viewModel);

            _modelCollection.CollectionChanged += ModelCollectionChanged;
        }

        public void ClearAllViewModelBindings()
        {
            if (_viewModelCollection != null)
            {
                _viewModelCollection.ToList().ForEach(DetachViewModel);
            }
        }

        public void Sort(Comparison<TModel> comparison)
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
    }
}
