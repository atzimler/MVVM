using System.Collections.Generic;

namespace ATZ.MVVM.ViewModels.Utility
{
    public interface ICollectionChangedEventSource<TSourceItem, TCollectionItem>
    {
        IEnumerable<TSourceItem> CollectionItemSource { get; }

        void ClearCollection();
        void AddItem(TCollectionItem item);
        TCollectionItem CreateItem(TSourceItem sourceItem);
        void InsertItem(int index, TCollectionItem item);
        void MoveItem(int oldIndex, int newIndex);
        void RemoveItem(int index);
        void ReplaceItem(int index, TCollectionItem newItem);
    }
}