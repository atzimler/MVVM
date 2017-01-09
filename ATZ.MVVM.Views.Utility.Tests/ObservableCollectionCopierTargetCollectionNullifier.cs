using ATZ.MVVM.Views.Utility.Connectors;
using System.Collections.ObjectModel;

namespace ATZ.MVVM.Views.Utility.Tests
{
    public class ObservableCollectionCopierTargetCollectionNullifier : ObservableCollectionCopierConnector<int, int>
    {
        public ObservableCollectionCopierTargetCollectionNullifier()
            : base(n => n)
        {
            SourceCollection = new ObservableCollection<int> { 1, 2, 3 };
            TargetCollection = new ObservableCollection<int>();
        }

        public override void ReplaceItem(int index, int newItem)
        {
            TargetCollection = null;
            base.ReplaceItem(index, newItem);
        }
    }
}
