using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using static System.Collections.Specialized.NotifyCollectionChangedAction;

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
                    _targetCollection.Clear();
                    foreach (S obj in _sourceCollection)
                    {
                        _targetCollection.Add(_transformSourceToTarget(obj));
                    }
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
            _sourceCollection.ToList().ForEach(obj => _targetCollection.Add(_transformSourceToTarget(obj)));
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case Add:
                    int insertPosition = e.NewStartingIndex;
                    foreach (S obj in e.NewItems)
                    {
                        _targetCollection.Insert(insertPosition++, _transformSourceToTarget(obj));
                    }
                    break;

                case Move:
                    {
                        var obj = _targetCollection[e.OldStartingIndex];
                        _targetCollection.RemoveAt(e.OldStartingIndex);
                        _targetCollection.Insert(e.NewStartingIndex, obj);
                    }
                    break;

                case Remove:
                    foreach (S obj in e.OldItems)
                    {
                        _targetCollection.RemoveAt(e.OldStartingIndex);
                    }
                    break;

                case Reset:
                    _targetCollection.Clear();
                    CopyObjects();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}
