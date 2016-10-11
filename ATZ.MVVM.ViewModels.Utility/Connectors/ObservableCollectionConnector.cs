using System;
using System.Collections.ObjectModel;

namespace ATZ.MVVM.ViewModels.Utility.Connectors
{
    public abstract class ObservableCollectionConnector<TSource, TTarget> :
        BaseConnector<TSource, TTarget, ObservableCollection<TTarget>>
    {
        public override void AddItem(TTarget item)
        {
            TargetCollection.Add(item);
        }

        public override void InsertItem(int index, TTarget item)
        {
            TargetCollection.Insert(index, item);
        }

        public override void MoveItem(int oldIndex, int newIndex)
        {
            TargetCollection.Move(oldIndex, newIndex);
        }
    }
}