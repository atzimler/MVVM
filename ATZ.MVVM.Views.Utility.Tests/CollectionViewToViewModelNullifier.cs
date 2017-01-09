using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Connectors;
using ATZ.MVVM.Views.Utility.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ATZ.MVVM.Views.Utility.Tests
{
    public class CollectionViewToViewModelNullifier : CollectionViewToViewModelConnector<TestView, TestModel>
    {
        private readonly StackPanel _stackPanel = new StackPanel();

        public CollectionViewToViewModelNullifier()
        {
            ViewModelCollection = new ObservableCollection<IViewModel<TestModel>> { new TestViewModel() };
            ViewCollection = _stackPanel?.Children;
        }

        public override void MoveItem(int oldIndex, int newIndex)
        {
            ViewCollection = null;

            base.MoveItem(oldIndex, newIndex);
        }

        public override void ReplaceItem(int index, IView<IViewModel<TestModel>> newItem)
        {
            ViewCollection = null;

            base.ReplaceItem(index, newItem);
        }
    }
}
