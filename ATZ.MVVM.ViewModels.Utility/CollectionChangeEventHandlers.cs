using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATZ.MVVM.ViewModels.Utility.Connectors;

namespace ATZ.MVVM.ViewModels.Utility
{
    public class CollectionChangeEventHandlers<TEventItem, TCollectionItem>
    {
        private readonly Action _clearCollection;
        private readonly Func<IEnumerable<TEventItem>> _collectionItemSource;
        private readonly Func<TEventItem, TCollectionItem> _createItem;
        private readonly Action<TCollectionItem> _addItem;
        private readonly Action<int, TCollectionItem> _insertItem;
        private readonly Action<int, int> _moveItem;
        private readonly Action<int> _removeItem;

        public CollectionChangeEventHandlers(
            Func<IEnumerable<TEventItem>> collectionItemSource, Action clearCollection,
            Func<TEventItem, TCollectionItem> createItem, Action<TCollectionItem> addItem, Action<int, TCollectionItem> insertItem, Action<int, int> moveItem, Action<int> removeItem
            )
        {
            _clearCollection = clearCollection;
            _collectionItemSource = collectionItemSource;
            _createItem = createItem;
            _addItem = addItem;
            _insertItem = insertItem;
            _moveItem = moveItem;
            _removeItem = removeItem;
        }

        public void Add(NotifyCollectionChangedEventArgs e)
        {
            var insertPosition = e.NewStartingIndex;
            foreach (TEventItem model in e.NewItems)
            {
                _insertItem(insertPosition++, _createItem(model));
            }
        }

        public void Move(NotifyCollectionChangedEventArgs e)
        {
            _moveItem(e.OldStartingIndex, e.NewStartingIndex);
        }

        public void Remove(NotifyCollectionChangedEventArgs e)
        {
            var itemsToRemove = e.OldItems.Count;
            while (itemsToRemove-- > 0)
            {
                _removeItem(e.OldStartingIndex);
            }
        }

        public void Reset(NotifyCollectionChangedEventArgs e)
        {
            _clearCollection();
            foreach (var model in _collectionItemSource())
            {
                _addItem(_createItem(model));
            }
        }


    }
}
