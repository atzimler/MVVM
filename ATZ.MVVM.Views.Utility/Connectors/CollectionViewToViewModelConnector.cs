using ATZ.DependencyInjection;
using ATZ.DependencyInjection.System;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Connectors;
using ATZ.MVVM.Views.Utility.Interfaces;
using Ninject;
using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    /// <summary>
    /// Connector between Views and ViewModels collections.
    /// </summary>
    /// <typeparam name="TView">The type of the View.</typeparam>
    /// <typeparam name="TModel">The type of the Model.</typeparam>
    public class CollectionViewToViewModelConnector<TView, TModel> : BaseConnector<IViewModel<TModel>, IView<IViewModel<TModel>>, IList>
        where TView : IView<IViewModel<TModel>>
        where TModel : class
    {
        /// <summary>
        /// The View collection.
        /// </summary>
        public IList ViewCollection
        {
            get { return TargetCollection; }
            set { TargetCollection = value; }
        }

        /// <summary>
        /// The ViewModel collection.
        /// </summary>
        public ObservableCollection<IViewModel<TModel>> ViewModelCollection
        {
            get { return SourceCollection; }
            set { SourceCollection = value; }
        }

        private static IView<IViewModel<TModel>> CreateViewForViewModel(IViewModel<TModel> viewModel)
        {
            var obj = DependencyResolver.Instance.GetInterface(typeof(IView<>), viewModel.GetType());
            if (obj == null)
            {
                DependencyResolver.Instance.Get<IDebug>().WriteLine(
                    $"IView<{typeof(IViewModel<TModel>).FullName}> created by the DependencyResolver.Instance is not of type {typeof(TView)}!");
                return Activator.CreateInstance<TView>();
            }

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

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ClearCollection"/>
        public override void ClearCollection() => TargetCollection.Clear();

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.AddItem"/>
        public override void AddItem(IView<IViewModel<TModel>> item) => TargetCollection.Add(item.UIElement);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.CreateItem"/>
        public override IView<IViewModel<TModel>> CreateItem(IViewModel<TModel> sourceItem) => CreateViewForViewModel(sourceItem);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.InsertItem"/>
        public override void InsertItem(int index, IView<IViewModel<TModel>> item) => TargetCollection.Insert(index, item.UIElement);

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
        public override void ReplaceItem(int index, IView<IViewModel<TModel>> newItem)
        {
            TargetCollection.RemoveAt(index);
            TargetCollection.Insert(index, newItem.UIElement);
        }
    }
}
