using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ATZ.MVVM.ViewModels.Utility
{
    public interface ICollectionChangedEventSource
    {
        void ClearCollection();
    }

    public class CollectionChangedEventHandlers<TEventItem, TCollectionItem>
    {
        private readonly Func<IEnumerable<TEventItem>> _collectionItemSource;
        private readonly Func<TEventItem, TCollectionItem> _createItem;
        private readonly Action<TCollectionItem> _addItem;
        private readonly Action<int, TCollectionItem> _insertItem;
        private readonly Action<int, int> _moveItem;
        private readonly Action<int> _removeItem;
        private readonly Action<int, TCollectionItem> _replaceItem;

        private readonly Dictionary<NotifyCollectionChangedAction, Action<ICollectionChangedEventSource, NotifyCollectionChangedEventArgs>>
            _eventHandlers;

        public CollectionChangedEventHandlers(
            Func<IEnumerable<TEventItem>> collectionItemSource,
            Func<TEventItem, TCollectionItem> createItem, Action<TCollectionItem> addItem, Action<int, TCollectionItem> insertItem, Action<int, int> moveItem, Action<int> removeItem,
            Action<int, TCollectionItem> replaceItem)
        {
                // TODO: Create these on an interface, make that the sender, then the dictionary and the partial handlers can be static.
                _collectionItemSource = collectionItemSource;
                _createItem = createItem;
                _addItem = addItem;
                _insertItem = insertItem;
                _moveItem = moveItem;
                _removeItem = removeItem;
                _replaceItem = replaceItem;

            _eventHandlers = new Dictionary<NotifyCollectionChangedAction, Action<ICollectionChangedEventSource, NotifyCollectionChangedEventArgs>>
                {
                    {NotifyCollectionChangedAction.Add, Add},
                    {NotifyCollectionChangedAction.Move, Move},
                    {NotifyCollectionChangedAction.Remove, Remove},
                    {NotifyCollectionChangedAction.Replace, Replace},
                    {NotifyCollectionChangedAction.Reset, Reset}
                };
        }

        private void Add(ICollectionChangedEventSource sender, NotifyCollectionChangedEventArgs e)
        {
            var insertPosition = e.NewStartingIndex;
            foreach (TEventItem model in e.NewItems)
            {
                _insertItem(insertPosition++, _createItem(model));
            }
        }

        private void Move(ICollectionChangedEventSource sender, NotifyCollectionChangedEventArgs e)
        {
            _moveItem(e.OldStartingIndex, e.NewStartingIndex);
        }

        private void Remove(ICollectionChangedEventSource sender, NotifyCollectionChangedEventArgs e)
        {
            var itemsToRemove = e.OldItems.Count;
            while (itemsToRemove-- > 0)
            {
                _removeItem(e.OldStartingIndex);
            }
        }

        private void Reset(ICollectionChangedEventSource sender, NotifyCollectionChangedEventArgs e)
        {
            sender.ClearCollection();
            foreach (var model in _collectionItemSource())
            {
                _addItem(_createItem(model));
            }
        }

        private void Replace(ICollectionChangedEventSource sender, NotifyCollectionChangedEventArgs e)
        {
            _replaceItem(e.OldStartingIndex, _createItem((TEventItem)e.OldItems[0]));
        }

        public void Handle(ICollectionChangedEventSource sender, NotifyCollectionChangedEventArgs e)
        {
            if (_eventHandlers.ContainsKey(e.Action))
            {
                _eventHandlers[e.Action](sender, e);
            }
        }
    }
}
