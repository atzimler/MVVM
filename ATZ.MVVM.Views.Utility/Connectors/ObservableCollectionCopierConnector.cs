using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ATZ.MVVM.ViewModels.Utility;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class ObservableCollectionCopierConnector<TSource, TTarget> : ICollectionChangedEventSource<TSource, TTarget>
    {
        private ObservableCollection<TSource> _sourceCollection;
        private ObservableCollection<TTarget> _targetCollection;
        private readonly Func<TSource, TTarget> _transformSourceToTarget;

        public ObservableCollection<TSource> SourceCollection
        {
            get { return _sourceCollection; }
            set
            {
                if (_sourceCollection != null)
                {
                    _sourceCollection.CollectionChanged -= SourceCollectionChanged;
                }

                _sourceCollection = value;
                CopyObjects();

                if (_sourceCollection != null)
                {
                    _sourceCollection.CollectionChanged += SourceCollectionChanged;
                }
            }
        }

        public ObservableCollection<TTarget> TargetCollection
        {
            get { return _targetCollection; }
            set
            {
                if (_targetCollection == value)
                {
                    return;
                }

                _targetCollection = value;

                if (_targetCollection != null)
                {
                    CopyObjects();
                }
            }
        }

        public ObservableCollectionCopierConnector(Func<TSource, TTarget> transformSourceToTarget)
        {
            _transformSourceToTarget = transformSourceToTarget;
        }

        private void CopyObjects()
        {
            if (_sourceCollection == null || _targetCollection == null)
            {
                return;
            }

            _targetCollection.Clear();
            foreach (var item in _sourceCollection)
            {
                _targetCollection.Add(_transformSourceToTarget(item));

            }
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_sourceCollection == null || _targetCollection == null)
            {
                return;
            }
            CollectionChangedEventHandlers<TSource, TTarget>.Handle(this, e);
        }

        #region ICollectionChangedEventSource<TSource, TTarget>

        IEnumerable<TSource> ICollectionChangedEventSource<TSource, TTarget>.CollectionItemSource => _sourceCollection;

        void ICollectionChangedEventSource<TSource, TTarget>.ClearCollection() => _targetCollection.Clear();

        void ICollectionChangedEventSource<TSource, TTarget>.AddItem(TTarget item) => _targetCollection.Add(item);

        TTarget ICollectionChangedEventSource<TSource, TTarget>.CreateItem(TSource sourceItem)
            => _transformSourceToTarget(sourceItem);

        void ICollectionChangedEventSource<TSource, TTarget>.InsertItem(int index, TTarget item)
            => _targetCollection.Insert(index, item);

        void ICollectionChangedEventSource<TSource, TTarget>.MoveItem(int oldIndex, int newIndex)
            => _targetCollection.Move(oldIndex, newIndex);

        void ICollectionChangedEventSource<TSource, TTarget>.RemoveItem(int index) => _targetCollection.RemoveAt(index);

        void ICollectionChangedEventSource<TSource, TTarget>.ReplaceItem(int index, TTarget newItem)
            => _targetCollection[index] = newItem;
        #endregion
    }
}
