using System;
using System.Collections.ObjectModel;
using ATZ.MVVM.ViewModels.Utility.Connectors;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class ObservableCollectionCopierConnector<TSource, TTarget> : ObservableCollectionConnector<TSource, TTarget>
    {
        public new ObservableCollection<TSource> SourceCollection
        {
            get { return base.SourceCollection; }
            set { base.SourceCollection = value; }
        }

        public new ObservableCollection<TTarget> TargetCollection
        {
            get { return base.TargetCollection; }
            set { base.TargetCollection = value; }
        }

        private readonly Func<TSource, TTarget> _transformSourceToTarget;

        public ObservableCollectionCopierConnector(Func<TSource, TTarget> transformSourceToTarget)
        {
            _transformSourceToTarget = transformSourceToTarget;
        }

        public override void ClearCollection() => TargetCollection.Clear();

        public override TTarget CreateItem(TSource sourceItem) => _transformSourceToTarget(sourceItem);

        public override void RemoveItem(int index) => TargetCollection.RemoveAt(index);

        public override void ReplaceItem(int index, TTarget newItem) => TargetCollection[index] = newItem;
    }
}
