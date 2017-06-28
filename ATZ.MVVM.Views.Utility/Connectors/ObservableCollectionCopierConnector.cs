using ATZ.CollectionObservers;
using JetBrains.Annotations;
using System;
using System.Collections.ObjectModel;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    /// <summary>
    /// Mirror an ObservableCollection into another ObservableCollection with a transformation of the source object to the mirror object.
    /// </summary>
    /// <typeparam name="TSource">The type of the items in the source collection.</typeparam>
    /// <typeparam name="TTarget">The type of the items in the mirror collection.</typeparam>
    public class ObservableCollectionCopierConnector<TSource, TTarget> : CollectionObserver<TSource, TTarget>
    {
        /// <summary>
        /// The source collection.
        /// </summary>
        public new ObservableCollection<TSource> SourceCollection
        {
            get => base.SourceCollection;
            set => base.SourceCollection = value;
        }

        /// <summary>
        /// The mirror collection.
        /// </summary>
        public new ObservableCollection<TTarget> TargetCollection
        {
            get => base.TargetCollection;
            set => base.TargetCollection = value;
        }

        [NotNull]
        private readonly Func<TSource, TTarget> _transformSourceToTarget;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transformSourceToTarget">The transformation from the source item to the mirror item.</param>
        public ObservableCollectionCopierConnector([NotNull] Func<TSource, TTarget> transformSourceToTarget)
        {
            _transformSourceToTarget = transformSourceToTarget;
        }

        /// <summary>
        /// Clear the target collection.
        /// </summary>
        public override void ClearCollection() => TargetCollection?.Clear();

        /// <summary>
        /// Create a new item for the target collection that will be associated with the given item in the source collection.
        /// </summary>
        /// <param name="sourceItem">The item in the source collection with which the newly created item should be associated.</param>
        /// <returns>The newly create item.</returns>
        public override TTarget CreateItem(TSource sourceItem) => _transformSourceToTarget(sourceItem);

        /// <summary>
        /// Remove an item from the target collection.
        /// </summary>
        /// <param name="index">The index of the item to be removed.</param>
        public override void RemoveItem(int index) => TargetCollection?.RemoveAt(index);

        /// <summary>
        /// Replace an item in the target collection.
        /// </summary>
        /// <param name="index">The index of the current item.</param>
        /// <param name="newItem">The new item.</param>
        public override void ReplaceItem(int index, TTarget newItem)
        {
            var collection = TargetCollection;
            if (collection == null)
            {
                return;
            }

            collection[index] = newItem;
        }
    }
}
