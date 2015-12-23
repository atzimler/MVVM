using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class CollectionViewToViewModelConnector<V, VM, M>
        where V : UIElement, IView<VM>, new ()
        where VM : BaseViewModel<M>
        where M : class
    {
        #region Private Variables
        private UIElementCollection _viewCollection;
        private ObservableCollection<VM> _viewModelCollection;
        #endregion

        #region Public Properties
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

        public ObservableCollection<VM> ViewModelCollection
        {
            get { return _viewModelCollection; }
            set
            {
                if (_viewModelCollection != value)
                {
                    UnbindViewModelCollection();

                    _viewModelCollection = value;

                    BindViewModelCollection();
                }
            }
        }
        #endregion

        #region Private Methods
        private void BindViewModelCollection()
        {
            if (_viewModelCollection != null)
            {
                _viewModelCollection.CollectionChanged += ViewModelCollectionChanged;
                ResetViewCollectionToViewModelCollection();
            }
        }

        private V CreateViewForViewModel(VM viewModel)
        {
            V view = new V();

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

            // TODO: This needs Unit testing.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    int insertPosition = e.NewStartingIndex;
                    foreach (VM viewModel in e.NewItems)
                    {
                        _viewCollection.Insert(insertPosition++, CreateViewForViewModel(viewModel));
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    UIElement uiElement = _viewCollection[e.OldStartingIndex];
                    _viewCollection.RemoveAt(e.OldStartingIndex);
                    _viewCollection.Insert(e.NewStartingIndex, uiElement);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (VM viewModel in e.OldItems)
                    {
                        _viewCollection.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _viewCollection.Clear();
                    foreach (VM viewModel in _viewModelCollection)
                    {
                        _viewCollection.Add(CreateViewForViewModel(viewModel));
                    }
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("CollectionViewToViewModelConnector: {0}", e.Action);
                    break;
            }
        }
        #endregion
    }
}
