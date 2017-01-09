using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ATZ.MVVM.ViewModels.Utility.Connectors
{
    /// <summary>
    /// Abstract base class to provide connectors between collections.
    /// </summary>
    /// <typeparam name="TSource">The type of the items in the source collection.</typeparam>
    /// <typeparam name="TTarget">The type of the items in the mirror collection.</typeparam>
    /// <typeparam name="TTargetCollection">The type of the mirror collection.</typeparam>
    public abstract class BaseConnector<TSource, TTarget, TTargetCollection> : ObservableObject, ICollectionChangedEventSource<TSource, TTarget>
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

        /// <summary>
        /// The source collection.
        /// </summary>
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

        /// <summary>
        /// The mirror collection.
        /// </summary>
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

        /// <summary>
        /// The mirror collection.
        /// </summary>
        public IEnumerable<TSource> CollectionItemSource => SourceCollection;

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ClearCollection"/>
        public abstract void ClearCollection();

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.AddItem"/>
        public abstract void AddItem(TTarget item);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.CreateItem"/>
        public abstract TTarget CreateItem(TSource sourceItem);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.InsertItem"/>
        public abstract void InsertItem(int index, TTarget item);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.MoveItem"/>
        public abstract void MoveItem(int oldIndex, int newIndex);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.RemoveItem"/>
        public abstract void RemoveItem(int index);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ReplaceItem"/>
        public abstract void ReplaceItem(int index, TTarget newItem);

        /// <summary>
        /// Add an item to the source and mirror collection at the same time.
        /// </summary>
        /// <param name="sourceItem">The item to add to the source collection.</param>
        /// <param name="targetItem">The item to add to the mirror collection.</param>
        public void Add(TSource sourceItem, TTarget targetItem)
        {
            if (_sourceCollection == null || _targetCollection == null)
            {
                return;
            }

            _sourceCollection.CollectionChanged -= SourceCollectionChanged;

            _sourceCollection.Add(sourceItem);
            AddItem(targetItem);

            if (_sourceCollection == null)
            {
                return;
            }

            _sourceCollection.CollectionChanged += SourceCollectionChanged;
        }

    }
}