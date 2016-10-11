using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ATZ.MVVM.ViewModels.Utility.Connectors
{
    public abstract class BaseConnector<TSource, TTarget, TTargetCollection> : ICollectionChangedEventSource<TSource, TTarget>
    {
        private ObservableCollection<TSource> _sourceCollection;
        private TTargetCollection _targetCollection;

        private void ResetTargetCollection()
        {
            SourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

        protected TTargetCollection TargetCollection
        {
            get { return _targetCollection; }
            set
            {
                if (ReferenceEquals(_targetCollection, value))
                {
                    return;
                }

                _targetCollection = value;
                ResetTargetCollection();
            }
        }

        public IEnumerable<TSource> CollectionItemSource => SourceCollection;
        public abstract void ClearCollection();
        public abstract void AddItem(TTarget item);
        public abstract TTarget CreateItem(TSource sourceItem);
        public abstract void InsertItem(int index, TTarget item);
        public abstract void MoveItem(int oldIndex, int newIndex);
        public abstract void RemoveItem(int index);
        public abstract void ReplaceItem(int index, TTarget newItem);
    }
}