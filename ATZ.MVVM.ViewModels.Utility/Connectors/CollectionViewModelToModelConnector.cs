using System;
using System.Collections.Generic;
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
            UnbindViewModel?.Invoke(viewModel);
        }

#region generic NotifyCollectionChangedEvent handlers
        private static void Add(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            var insertPosition = e.NewStartingIndex;
            foreach (TModel model in e.NewItems)
            {
                sender._viewModelCollection.Insert(insertPosition++, sender.CreateViewModelForModel(model));
            }
        }
        private static void Move(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            sender._viewModelCollection.Move(e.OldStartingIndex, e.NewStartingIndex);
        }

        private static void Remove(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            var itemsToRemove = e.OldItems.Count;
            while (itemsToRemove-- > 0)
            {
                sender.DetachViewModel(sender._viewModelCollection[e.OldStartingIndex]);
                sender._viewModelCollection.RemoveAt(e.OldStartingIndex);
            }
        }

        private static void Reset(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            sender.ClearViewModelCollection();
            foreach (TModel model in sender._modelCollection)
            {
                sender._viewModelCollection.Add(sender.CreateViewModelForModel(model));
            }
        }

        private static void Replace(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            sender.DetachViewModel(sender._viewModelCollection[e.OldStartingIndex]);
            sender._viewModelCollection[e.OldStartingIndex] =
                sender.CreateViewModelForModel(sender._modelCollection[e.OldStartingIndex]);
        }

        private static readonly Dictionary<NotifyCollectionChangedAction, Action<CollectionViewModelToModelConnector<TViewModel, TModel>, NotifyCollectionChangedEventArgs>> EventHandlers =
            new Dictionary<NotifyCollectionChangedAction, Action<CollectionViewModelToModelConnector<TViewModel, TModel>, NotifyCollectionChangedEventArgs>>
            {
                {NotifyCollectionChangedAction.Add, Add},
                {NotifyCollectionChangedAction.Move, Move},
                {NotifyCollectionChangedAction.Remove, Remove},
                {NotifyCollectionChangedAction.Replace, Replace},
                {NotifyCollectionChangedAction.Reset, Reset}
            };
#endregion

        private void ModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_modelCollection == null || _viewModelCollection == null)
            {
                return;
            }

            // TODO: This needs unit testing.
            // TODO: This and Views.Utility.CollectionConnector share similarities, maybe they can be merged.
            if (EventHandlers.ContainsKey(e.Action))
            {
                EventHandlers[e.Action](this, e);
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
