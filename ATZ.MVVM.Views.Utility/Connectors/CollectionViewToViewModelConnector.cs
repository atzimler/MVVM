using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Connectors;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class CollectionViewToViewModelConnector<TView, TViewModel, TModel> : BaseConnector<TViewModel, TView, UIElementCollection>
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

        private static TView CreateViewForViewModel(TViewModel viewModel)
        {
            // TODO: This should be DependencyInjection for IView<VM> with proper insertion of the current type of the viewModel, so that different types of subclasses can be handled correctly.
            TView view = new TView();

            // TODO: This should be IView<VM>
            //view.ViewModel = viewModel;
            view.SetViewModel(viewModel);

            return view;
        }

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
