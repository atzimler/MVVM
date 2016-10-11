using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class CollectionViewToViewModelConnector<TView, TViewModel, TModel> : ICollectionChangedEventSource<TViewModel, TView>
        where TView : UIElement, IView<TViewModel>, new ()
        where TViewModel : BaseViewModel<TModel>
        where TModel : class
    {
        private UIElementCollection _viewCollection;
        private ObservableCollection<TViewModel> _viewModelCollection;

        public UIElementCollection ViewCollection
        {
            get { return _viewCollection; }
            set
            {
                if (_viewCollection != value)
                {
                    _viewCollection = value;
                    ResetViewCollectionToViewModelCollection();
                }
            }
        }

        // TODO: Source collection handling can be merged between the three connector classes.
        public ObservableCollection<TViewModel> ViewModelCollection
        {
            get { return _viewModelCollection; }
            set
            {
                if (_viewModelCollection == value)
                {
                    return;
                }

                UnbindViewModelCollection();

                _viewModelCollection = value;

                BindViewModelCollection();
            }
        }

        private void BindViewModelCollection()
        {
            if (_viewModelCollection != null)
            {
                _viewModelCollection.CollectionChanged += ViewModelCollectionChanged;
                ResetViewCollectionToViewModelCollection();
            }
        }

        private static TView CreateViewForViewModel(TViewModel viewModel)
        {
            // TODO: This should be DependencyInjection for IView<VM> with proper insertion of the current type of the viewModel, so that different types of subclasses can be handled correctly.
            TView view = new TView();

            // TODO: This should be IView<VM>
            //view.ViewModel = viewModel;
            // TODO: This should be automatically done by the ViewExtensions.SetViewModel.
            //view.BindModel(viewModel);
            view.SetViewModel(viewModel);

            return view;
        }

        private void ResetViewCollectionToViewModelCollection()
        {
            ViewModelCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void UnbindViewModelCollection()
        {
            if (_viewModelCollection != null)
            {
                _viewModelCollection.CollectionChanged -= ViewModelCollectionChanged;
            }
        }

        private void ViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_viewModelCollection == null || _viewCollection == null)
            {
                return;
            }

            CollectionChangedEventHandlers<TViewModel, TView>.Handle(this, e);
        }

        #region ICollectionChangedEventSource<TViewModel, TView>
        IEnumerable<TViewModel> ICollectionChangedEventSource<TViewModel, TView>.CollectionItemSource => _viewModelCollection;

        void ICollectionChangedEventSource<TViewModel, TView>.ClearCollection() => _viewCollection.Clear();

        void ICollectionChangedEventSource<TViewModel, TView>.AddItem(TView item) => _viewCollection.Add(item);

        TView ICollectionChangedEventSource<TViewModel, TView>.CreateItem(TViewModel sourceItem)
            => CreateViewForViewModel(sourceItem);

        void ICollectionChangedEventSource<TViewModel, TView>.InsertItem(int index, TView item)
            => _viewCollection.Insert(index, item);

        void ICollectionChangedEventSource<TViewModel, TView>.MoveItem(int oldIndex, int newIndex)
        {
            var uiElement = _viewCollection[oldIndex];
            _viewCollection.RemoveAt(oldIndex);
            _viewCollection.Insert(newIndex, uiElement);
        }

        void ICollectionChangedEventSource<TViewModel, TView>.RemoveItem(int index) => _viewCollection.RemoveAt(index);

        void ICollectionChangedEventSource<TViewModel, TView>.ReplaceItem(int index, TView newItem)
        {
            _viewCollection.RemoveAt(index);
            _viewCollection.Insert(index, newItem);
        }
        #endregion

    }
}
