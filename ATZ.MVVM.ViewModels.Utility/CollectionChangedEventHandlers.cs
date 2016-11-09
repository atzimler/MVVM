using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ATZ.MVVM.ViewModels.Utility
{
    /// <summary>
    /// Generic implementation for handling collection change events in ObservableCollections when mirroring the collection.
    /// </summary>
    /// <typeparam name="TEventItem">The type of the items in the event from the source ObservableCollection.</typeparam>
    /// <typeparam name="TCollectionItem">The type of the items in the mirror ObservableCollection.</typeparam>
    public static class CollectionChangedEventHandlers<TEventItem, TCollectionItem>
    {
        private static readonly Dictionary<NotifyCollectionChangedAction, Action<ICollectionChangedEventSource<TEventItem, TCollectionItem>, NotifyCollectionChangedEventArgs>>
            EventHandlers = new Dictionary<NotifyCollectionChangedAction, Action<ICollectionChangedEventSource<TEventItem, TCollectionItem>, NotifyCollectionChangedEventArgs>> {
                    {NotifyCollectionChangedAction.Add, Add},
                    {NotifyCollectionChangedAction.Move, Move},
                    {NotifyCollectionChangedAction.Remove, Remove},
                    {NotifyCollectionChangedAction.Replace, Replace},
                    {NotifyCollectionChangedAction.Reset, Reset}
                };

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
            sender.ReplaceItem(e.NewStartingIndex, sender.CreateItem((TEventItem)e.NewItems[0]));
        }

        /// <summary>
        /// Handle INotifyCollectionChanged events.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        public static void Handle(ICollectionChangedEventSource<TEventItem, TCollectionItem> sender, NotifyCollectionChangedEventArgs e)
        {
            if (EventHandlers.ContainsKey(e.Action))
            {
                EventHandlers[e.Action](sender, e);
            }
        }
    }
}
