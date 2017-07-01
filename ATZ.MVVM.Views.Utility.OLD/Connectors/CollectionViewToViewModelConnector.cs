using ATZ.CollectionObservers;
using ATZ.DependencyInjection;
using ATZ.DependencyInjection.System;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;
using Ninject;
using System.Collections;
using System.Collections.ObjectModel;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    /// <summary>
    /// Connector between Views and ViewModels collections.
    /// </summary>
    /// <typeparam name="TModel">The type of the Model.</typeparam>
    public class CollectionViewToViewModelConnector<TModel> : CollectionObserverBase<IViewModel<TModel>, IView<IViewModel<TModel>>, IList> where TModel : class
    {
        /// <summary>
        /// The View collection.
        /// </summary>
        public IList ViewCollection
        {
            get => TargetCollection;
            set => TargetCollection = value;
        }

        /// <summary>
        /// The ViewModel collection.
        /// </summary>
        public ObservableCollection<IViewModel<TModel>> ViewModelCollection
        {
            get => SourceCollection;
            set => SourceCollection = value;
        }

        private static IView<IViewModel<TModel>> CreateViewForViewModel(IViewModel<TModel> viewModel)
        {
            if (viewModel == null)
            {
                DependencyResolver.Instance.Get<IDebug>().WriteLine("viewModel == null, cannot resolve appropriate view type without type information!");
                return null;
            }

            var obj = DependencyResolver.Instance.GetInterface(typeof(IView<>), viewModel.GetType());
            var view = obj as IView<IViewModel<TModel>>;
            if (view == null)
            {
                DependencyResolver.Instance.Get<IDebug>().WriteLine(
                    $"IView<{viewModel.GetType().FullName}> was successfully resolved, but it has no interface of IView<IViewModel<{typeof(TModel).FullName}>>!");
                return null;
            }

            view.SetViewModel(viewModel);
            return view;
        }

        /// <summary>
        /// Clear the view collection.
        /// </summary>
        public override void ClearCollection() => TargetCollection?.Clear();

        /// <summary>
        /// Add a view into the view collection.
        /// </summary>
        /// <param name="item">The item to add to the target collection.</param>
        public override void AddItem(IView<IViewModel<TModel>> item) => TargetCollection?.Add(item?.UIElement);

        /// <summary>
        /// Create a new view for the view model.
        /// </summary>
        /// <param name="viewModel">The view model for which a view should be created.</param>
        /// <returns>The newly created view.</returns>
        public override IView<IViewModel<TModel>> CreateItem(IViewModel<TModel> viewModel) => CreateViewForViewModel(viewModel);

        /// <summary>
        /// Insert a view at the specified location into the view collection.
        /// </summary>
        /// <param name="index">The index where the view will be added into the view collection.</param>
        /// <param name="item">The view that will be added to the view collection.</param>
        public override void InsertItem(int index, IView<IViewModel<TModel>> item) => TargetCollection?.Insert(index, item?.UIElement);

        /// <summary>
        /// Move a view in the view collection from an old position to a new position.
        /// </summary>
        /// <param name="oldIndex">The current index of the view.</param>
        /// <param name="newIndex">The new index of the view.</param>
        public override void MoveItem(int oldIndex, int newIndex)
        {
            var collection = TargetCollection;
            if (collection == null)
            {
                return;
            }

            var uiElement = collection[oldIndex];
            collection.RemoveAt(oldIndex);
            collection.Insert(newIndex, uiElement);
        }

        /// <summary>
        /// Remove a view from the view collection.
        /// </summary>
        /// <param name="index">The current index of the view.</param>
        public override void RemoveItem(int index) => TargetCollection?.RemoveAt(index);

        /// <summary>
        /// Replace a view in the view collection.
        /// </summary>
        /// <param name="index">The index of the current view.</param>
        /// <param name="newItem">The new view.</param>
        public override void ReplaceItem(int index, IView<IViewModel<TModel>> newItem)
        {
            var collection = TargetCollection;
            if (collection == null)
            {
                return;
            }

            collection.RemoveAt(index);
            collection.Insert(index, newItem?.UIElement);
        }
    }
}
