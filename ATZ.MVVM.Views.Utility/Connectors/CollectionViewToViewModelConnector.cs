using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ATZ.DependencyInjection;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Connectors;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class CollectionViewToViewModelConnector<TView, TViewModel, TModel> : BaseConnector<TViewModel, IView<TViewModel>, UIElementCollection>
        where TView : UIElement, IView<TViewModel>, new ()
        where TViewModel : BaseViewModel<TModel>
        where TModel : class
    {
        public UIElementCollection ViewCollection
        {
            get { return TargetCollection; }
            set { TargetCollection = value; }
        }

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
            return new TView();
        }

        public override void ClearCollection() => TargetCollection.Clear();
        public override void AddItem(IView<TViewModel> item) => TargetCollection.Add(item.UIElement);
        public override IView<TViewModel> CreateItem(TViewModel sourceItem) => CreateViewForViewModel(sourceItem);
        public override void InsertItem(int index, IView<TViewModel> item) => TargetCollection.Insert(index, item.UIElement);

        public override void MoveItem(int oldIndex, int newIndex)
        {
            var uiElement = TargetCollection[oldIndex];
            TargetCollection.RemoveAt(oldIndex);
            TargetCollection.Insert(newIndex, uiElement);
        }

        public override void RemoveItem(int index) => TargetCollection.RemoveAt(index);

        public override void ReplaceItem(int index, IView<TViewModel> newItem)
        {
            TargetCollection.RemoveAt(index);
            TargetCollection.Insert(index, newItem.UIElement);
        }
    }
}
