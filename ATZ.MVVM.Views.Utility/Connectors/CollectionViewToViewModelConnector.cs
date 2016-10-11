using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public abstract class Connector<TSource, TTarget, TTargetCollection> : ICollectionChangedEventSource<TSource, TTarget>
    {
        private ObservableCollection<TSource> _sourceCollection;

        private void ResetTargetCollection()
        {
            SourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        // TODO: This should be private, temporary solution till refactoring is completed.
        protected void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_sourceCollection == null || TargetCollection == null)
            {
                return;
            }

            CollectionChangedEventHandlers<TSource, TTarget>.Handle(this, e);
        }

        protected ObservableCollection<TSource> SourceCollection
        {
            get { return _sourceCollection; }
            set
            {
                if (_sourceCollection == value)
                {
                    return;
                }

                if (_sourceCollection != null)
                {
                    _sourceCollection.CollectionChanged -= SourceCollectionChanged;
                }

                _sourceCollection = value;
                ResetTargetCollection();

                if (_sourceCollection != null)
                {
                    _sourceCollection.CollectionChanged += SourceCollectionChanged;
                }
            }
        }

        protected TTargetCollection TargetCollection { get; set; }

        public IEnumerable<TSource> CollectionItemSource => SourceCollection;
        public abstract void ClearCollection();
        public abstract void AddItem(TTarget item);
        public abstract TTarget CreateItem(TSource sourceItem);
        public abstract void InsertItem(int index, TTarget item);
        public abstract void MoveItem(int oldIndex, int newIndex);
        public abstract void RemoveItem(int index);
        public abstract void ReplaceItem(int index, TTarget newItem);
    }

    public class CollectionViewToViewModelConnector<TView, TViewModel, TModel> : Connector<TViewModel, TView, UIElementCollection>
        where TView : UIElement, IView<TViewModel>, new ()
        where TViewModel : BaseViewModel<TModel>
        where TModel : class
    {
        public UIElementCollection ViewCollection
        {
            get { return TargetCollection; }
            set
            {
                if (TargetCollection == value)
                {
                    return;
                }

                TargetCollection = value;
                ResetViewCollectionToViewModelCollection();
            }
        }

        public ObservableCollection<TViewModel> ViewModelCollection
        {
            get { return SourceCollection; }
            set { SourceCollection = value; }
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
            SourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        // TODO: Check: we might be able to unify some of the ICollectionChangedEventSource members across Connectors to move them to the base.
        public override void ClearCollection() => TargetCollection.Clear();
        public override void AddItem(TView item) => TargetCollection.Add(item);
        public override TView CreateItem(TViewModel sourceItem) => CreateViewForViewModel(sourceItem);
        public override void InsertItem(int index, TView item) => TargetCollection.Insert(index, item);

        public override void MoveItem(int oldIndex, int newIndex)
        {
            var uiElement = TargetCollection[oldIndex];
            TargetCollection.RemoveAt(oldIndex);
            TargetCollection.Insert(newIndex, uiElement);
        }

        public override void RemoveItem(int index) => TargetCollection.RemoveAt(index);

        public override void ReplaceItem(int index, TView newItem)
        {
            TargetCollection.RemoveAt(index);
            TargetCollection.Insert(index, newItem);
        }
    }
}
