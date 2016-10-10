﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ATZ.MVVM.ViewModels.Utility
{
    public interface ICollectionChangedEventSource<TSourceItem, out TCollectionItem>
    {
        IEnumerable<TSourceItem> CollectionItemSource { get; }

        void ClearCollection();
        TCollectionItem CreateItem(TSourceItem sourceItem);
    }

    public class CollectionChangedEventHandlers<TEventItem, TCollectionItem>
    {
        private readonly Action<TCollectionItem> _addItem;
        private readonly Action<int, TCollectionItem> _insertItem;
        private readonly Action<int, int> _moveItem;
        private readonly Action<int> _removeItem;
        private readonly Action<int, TCollectionItem> _replaceItem;

        private readonly Dictionary<NotifyCollectionChangedAction, Action<ICollectionChangedEventSource<TEventItem, TCollectionItem>, NotifyCollectionChangedEventArgs>>
            _eventHandlers;

        public CollectionChangedEventHandlers(
            Action<TCollectionItem> addItem, Action<int, TCollectionItem> insertItem, Action<int, int> moveItem, Action<int> removeItem,
            Action<int, TCollectionItem> replaceItem)
        {
                // TODO: Create these on an interface, make that the sender, then the dictionary and the partial handlers can be static.
                _addItem = addItem;
                _insertItem = insertItem;
                _moveItem = moveItem;
                _removeItem = removeItem;
                _replaceItem = replaceItem;

            _eventHandlers = new Dictionary<NotifyCollectionChangedAction, Action<ICollectionChangedEventSource<TEventItem, TCollectionItem>, NotifyCollectionChangedEventArgs>>
                {
                    {NotifyCollectionChangedAction.Add, Add},
                    {NotifyCollectionChangedAction.Move, Move},
                    {NotifyCollectionChangedAction.Remove, Remove},
                    {NotifyCollectionChangedAction.Replace, Replace},
                    {NotifyCollectionChangedAction.Reset, Reset}
                };
        }

        private void Add(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            var insertPosition = e.NewStartingIndex;
            foreach (TEventItem model in e.NewItems)
            {
                _insertItem(insertPosition++, sender.CreateItem(model));
            }
        }

        private void Move(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            _moveItem(e.OldStartingIndex, e.NewStartingIndex);
        }

        private void Remove(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            var itemsToRemove = e.OldItems.Count;
            while (itemsToRemove-- > 0)
            {
                _removeItem(e.OldStartingIndex);
            }
        }

        private void Reset(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            sender.ClearCollection();
            foreach (var model in sender.CollectionItemSource)
            {
                _addItem(sender.CreateItem(model));
            }
        }

        private void Replace(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            _replaceItem(e.OldStartingIndex, sender.CreateItem((TEventItem)e.OldItems[0]));
        }

        public void Handle(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            if (_eventHandlers.ContainsKey(e.Action))
            {
                _eventHandlers[e.Action](sender, e);
            }
        }
    }
}
