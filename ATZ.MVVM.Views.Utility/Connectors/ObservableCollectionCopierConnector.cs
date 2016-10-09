using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class ObservableCollectionCopierConnector<TSource, TTarget>
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
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    int insertPosition = e.NewStartingIndex;
                    foreach (TSource obj in e.NewItems)
                    {
                        _targetCollection.Insert(insertPosition++, _transformSourceToTarget(obj));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    {
                        var obj = _targetCollection[e.OldStartingIndex];
                        _targetCollection.RemoveAt(e.OldStartingIndex);
                        _targetCollection.Insert(e.NewStartingIndex, obj);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (TSource obj in e.OldItems)
                    {
                        _targetCollection.RemoveAt(e.OldStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    _targetCollection[e.OldStartingIndex] =
                        _transformSourceToTarget(_sourceCollection[e.OldStartingIndex]);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    CopyObjects();
                    break;
            }
        }
    }
}
