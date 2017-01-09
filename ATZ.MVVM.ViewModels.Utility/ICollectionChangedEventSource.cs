using JetBrains.Annotations;
using System.Collections.Generic;

namespace ATZ.MVVM.ViewModels.Utility
{
    /// <summary>
    /// Provides methods to mirror an ObservableCollection in an other collection with associated types.
    /// </summary>
    /// <typeparam name="TSourceItem">The item type in the collection to observe.</typeparam>
    /// <typeparam name="TCollectionItem">The item type in the mirror collection.</typeparam>
    public interface ICollectionChangedEventSource<TSourceItem, TCollectionItem>
    {
        /// <summary>
        /// The Observable collection to mirror.
        /// </summary>
        IEnumerable<TSourceItem> CollectionItemSource { get; }

        /// <summary>
        /// Clear the mirror collection.
        /// </summary>
        void ClearCollection();

        /// <summary>
        /// Add an item to the mirror collection.
        /// </summary>
        /// <param name="item">The item to add to the mirror collection.</param>
        void AddItem(TCollectionItem item);

        /// <summary>
        /// Create an item for the mirror collection that will be associated with the source item.
        /// </summary>
        /// <param name="sourceItem">The source item for which the item should be associated with.</param>
        /// <returns>The item associated with the source item.</returns>
        TCollectionItem CreateItem([NotNull] TSourceItem sourceItem);

        /// <summary>
        /// Insert an item at the specified index into the mirror collection.
        /// </summary>
        /// <param name="index">The position where the item will be inserted.</param>
        /// <param name="item">The item to insert into the mirror collection.</param>
        void InsertItem(int index, TCollectionItem item);

        /// <summary>
        /// Move an item in the mirror collection.
        /// </summary>
        /// <param name="oldIndex">The index from which the item will be moved.</param>
        /// <param name="newIndex">The new index of the item moved.</param>
        void MoveItem(int oldIndex, int newIndex);

        /// <summary>
        /// Remove an item from the mirror collection.
        /// </summary>
        /// <param name="index">The index from the item to remove from the mirror collection.</param>
        void RemoveItem(int index);

        /// <summary>
        /// Replace an item in the mirror collection.
        /// </summary>
        /// <param name="index">The index at which the item in the mirror collection will be replaced.</param>
        /// <param name="newItem">The new item in the mirror collection.</param>
        void ReplaceItem(int index, TCollectionItem newItem);
    }
}