using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using ATZ.DependencyInjection;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Connectors;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    /// <summary>
    /// Connector between Views and ViewModels collections.
    /// </summary>
    /// <typeparam name="TView">The type of the View.</typeparam>
    /// <typeparam name="TViewModel">The type of the ViewModel.</typeparam>
    /// <typeparam name="TModel">The type of the Model.</typeparam>
    public class CollectionViewToViewModelConnector<TView, TViewModel, TModel> : BaseConnector<TViewModel, IView<TViewModel>, UIElementCollection>
        where TView : IView<TViewModel>
        where TViewModel : BaseViewModel<TModel>
        where TModel : class
    {
        /// <summary>
        /// The View collection.
        /// </summary>
        public UIElementCollection ViewCollection
        {
            get { return TargetCollection; }
            set { TargetCollection = value; }
        }

        /// <summary>
        /// The ViewModel collection.
        /// </summary>
        public ObservableCollection<TViewModel> ViewModelCollection
        {
            get { return SourceCollection; }
            set { SourceCollection = value; }
        }

        private static IView<TViewModel> CreateViewForViewModel(TViewModel viewModel)
        {
            var view = DependencyResolver.Instance.GetInterface<IView<TViewModel>>(typeof(IView<>), viewModel.GetType());
            view.SetViewModel(viewModel);

            if (view != null)
            {
                return view;
            }

            Debug.WriteLine($"IView<{typeof(TViewModel).FullName}> created by the DependencyResolver.Instance is not of type {typeof(TView)}!");
            return Activator.CreateInstance<TView>();
        }

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ClearCollection"/>
        public override void ClearCollection() => TargetCollection.Clear();

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.AddItem"/>
        public override void AddItem(IView<TViewModel> item) => TargetCollection.Add(item.UIElement);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.CreateItem"/>
        public override IView<TViewModel> CreateItem(TViewModel sourceItem) => CreateViewForViewModel(sourceItem);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.InsertItem"/>
        public override void InsertItem(int index, IView<TViewModel> item) => TargetCollection.Insert(index, item.UIElement);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.MoveItem"/>
        public override void MoveItem(int oldIndex, int newIndex)
        {
            var uiElement = TargetCollection[oldIndex];
            TargetCollection.RemoveAt(oldIndex);
            TargetCollection.Insert(newIndex, uiElement);
        }

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.RemoveItem"/>
        public override void RemoveItem(int index) => TargetCollection.RemoveAt(index);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ReplaceItem"/>
        public override void ReplaceItem(int index, IView<TViewModel> newItem)
        {
            TargetCollection.RemoveAt(index);
            TargetCollection.Insert(index, newItem.UIElement);
        }
    }
}
