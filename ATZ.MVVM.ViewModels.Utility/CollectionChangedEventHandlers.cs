using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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

    public class CollectionChangedEventHandlers<TEventItem, TCollectionItem>
    {
        private readonly Dictionary<NotifyCollectionChangedAction, Action<ICollectionChangedEventSource<TEventItem, TCollectionItem>, NotifyCollectionChangedEventArgs>>
            _eventHandlers;

        public CollectionChangedEventHandlers()
        {
            _eventHandlers = new Dictionary<NotifyCollectionChangedAction, Action<ICollectionChangedEventSource<TEventItem, TCollectionItem>, NotifyCollectionChangedEventArgs>>
                {
                    {NotifyCollectionChangedAction.Add, Add},
                    {NotifyCollectionChangedAction.Move, Move},
                    {NotifyCollectionChangedAction.Remove, Remove},
                    {NotifyCollectionChangedAction.Replace, Replace},
                    {NotifyCollectionChangedAction.Reset, Reset}
                };
        }

        private static void Add(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            var insertPosition = e.NewStartingIndex;
            foreach (TEventItem model in e.NewItems)
            {
                sender.InsertItem(insertPosition++, sender.CreateItem(model));
            }
        }

        private static void Move(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            sender.MoveItem(e.OldStartingIndex, e.NewStartingIndex);
        }

        private static void Remove(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            var itemsToRemove = e.OldItems.Count;
            while (itemsToRemove-- > 0)
            {
                sender.RemoveItem(e.OldStartingIndex);
            }
        }

        private static void Reset(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            sender.ClearCollection();
            foreach (var model in sender.CollectionItemSource)
            {
                sender.AddItem(sender.CreateItem(model));
            }
        }

        private static void Replace(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            sender.ReplaceItem(e.OldStartingIndex, sender.CreateItem((TEventItem)e.OldItems[0]));
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
