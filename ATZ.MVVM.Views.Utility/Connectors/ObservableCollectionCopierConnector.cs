using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class ObservableCollectionCopierConnector<S, T>
    {
        #region Private Variables
        private ObservableCollection<S> _sourceCollection;
        private ObservableCollection<T> _targetCollection;
        private Func<S, T> _transformSourceToTarget;
        #endregion

        #region Public Properties
        public ObservableCollection<S> SourceCollection
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

        public ObservableCollection<T> TargetCollection
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
        #endregion

        #region Constructors
        public ObservableCollectionCopierConnector(Func<S, T> transformSourceToTarget)
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
                    foreach (S obj in e.NewItems)
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
                    foreach (S obj in e.OldItems)
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
        #endregion
    }
}
