﻿using System;
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
        public ViewModelBinder UnbindViewModel { get; set; }


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
            Action clearCollection = sender.ClearViewModelCollection;
            Func<IEnumerable<TModel>> collectionItemSource = () => sender._modelCollection;
            Func<TModel, TViewModel> create = sender.CreateViewModelForModel;
            Action<int, TViewModel> insert = sender._viewModelCollection.Insert;
            Action<TViewModel> add = sender._viewModelCollection.Add;
            Action<int, int> move = sender._viewModelCollection.Move;
            Action<int> remove = (index) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection.RemoveAt(index);
            };
            Action<int, TViewModel> replace = (index, newItem) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection[index] = newItem;
            };
            var collectionChangeEventHandlers = new CollectionChangeEventHandlers<TModel, TViewModel>(collectionItemSource, clearCollection, create, add, insert, move, remove, replace);
            collectionChangeEventHandlers.Add(e);
        }

        private static void Move(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            Action clearCollection = sender.ClearViewModelCollection;
            Func<IEnumerable<TModel>> collectionItemSource = () => sender._modelCollection;
            Func<TModel, TViewModel> create = sender.CreateViewModelForModel;
            Action<int, TViewModel> insert = sender._viewModelCollection.Insert;
            Action<TViewModel> add = sender._viewModelCollection.Add;
            Action<int, int> move = sender._viewModelCollection.Move;
            Action<int> remove = (index) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection.RemoveAt(index);
            };
            Action<int, TViewModel> replace = (index, newItem) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection[index] = newItem;
            };
            var collectionChangeEventHandlers = new CollectionChangeEventHandlers<TModel, TViewModel>(collectionItemSource, clearCollection, create, add, insert, move, remove, replace);
            collectionChangeEventHandlers.Move(e);
        }

        private static void Remove(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            Action clearCollection = sender.ClearViewModelCollection;
            Func<IEnumerable<TModel>> collectionItemSource = () => sender._modelCollection;
            Func<TModel, TViewModel> create = sender.CreateViewModelForModel;
            Action<int, TViewModel> insert = sender._viewModelCollection.Insert;
            Action<TViewModel> add = sender._viewModelCollection.Add;
            Action<int, int> move = sender._viewModelCollection.Move;
            Action<int> remove = (index) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection.RemoveAt(index);
            };
            Action<int, TViewModel> replace = (index, newItem) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection[index] = newItem;
            };
            var collectionChangeEventHandlers = new CollectionChangeEventHandlers<TModel, TViewModel>(collectionItemSource, clearCollection, create, add, insert, move, remove, replace);
            collectionChangeEventHandlers.Remove(e);
        }

        private static void Reset(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            Action clearCollection = sender.ClearViewModelCollection;
            Func<IEnumerable<TModel>> collectionItemSource = () => sender._modelCollection;
            Func<TModel, TViewModel> create = sender.CreateViewModelForModel;
            Action<int, TViewModel> insert = sender._viewModelCollection.Insert;
            Action<TViewModel> add = sender._viewModelCollection.Add;
            Action<int, int> move = sender._viewModelCollection.Move;
            Action<int> remove = (index) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection.RemoveAt(index);
            };
            Action<int, TViewModel> replace = (index, newItem) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection[index] = newItem;
            };
            var collectionChangeEventHandlers = new CollectionChangeEventHandlers<TModel, TViewModel>(collectionItemSource, clearCollection, create, add, insert, move, remove, replace);
            collectionChangeEventHandlers.Reset(e);
        }

        private static void Replace(CollectionViewModelToModelConnector<TViewModel, TModel> sender, NotifyCollectionChangedEventArgs e)
        {
            Action clearCollection = sender.ClearViewModelCollection;
            Func<IEnumerable<TModel>> collectionItemSource = () => sender._modelCollection;
            Func<TModel, TViewModel> create = sender.CreateViewModelForModel;
            Action<int, TViewModel> insert = sender._viewModelCollection.Insert;
            Action<TViewModel> add = sender._viewModelCollection.Add;
            Action<int, int> move = sender._viewModelCollection.Move;
            Action<int> remove = (index) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection.RemoveAt(index);
            };
            Action<int, TViewModel> replace = (index, newItem) =>
            {
                sender.DetachViewModel(sender._viewModelCollection[index]);
                sender._viewModelCollection[index] = newItem;
            };
            var collectionChangeEventHandlers = new CollectionChangeEventHandlers<TModel, TViewModel>(collectionItemSource, clearCollection, create, add, insert, move, remove, replace);
            collectionChangeEventHandlers.Replace(e);
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
            IsValid = _viewModelCollection?.ToList().TrueForAll(vm => vm.IsValid) ?? true;
        }

        protected virtual void OnIsValidChanged()
        {
            IsValidChanged?.Invoke(this, EventArgs.Empty);
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
            _viewModelCollection?.ToList().ForEach(DetachViewModel);
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
