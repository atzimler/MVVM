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
        private readonly Func<TEventItem, TCollectionItem> _create;
        private readonly Action<int, TCollectionItem> _insert;
        private readonly Action<int, int> _move;
        private readonly Action<int> _remove;

        public CollectionChangeEventHandlers(Func<TEventItem, TCollectionItem> create, Action<int, TCollectionItem> insert, Action<int, int> move, Action<int> remove)
        {
            _create = create;
            _insert = insert;
            _move = move;
            _remove = remove;
        }

        public void Add(NotifyCollectionChangedEventArgs e)
        {
            var insertPosition = e.NewStartingIndex;
            foreach (TEventItem model in e.NewItems)
            {
                _insert(insertPosition++, _create(model));
            }
        }

        public void Move(NotifyCollectionChangedEventArgs e)
        {
            _move(e.OldStartingIndex, e.NewStartingIndex);
        }

        public void Remove(NotifyCollectionChangedEventArgs e)
        {
            var itemsToRemove = e.OldItems.Count;
            while (itemsToRemove-- > 0)
            {
                _remove(e.OldStartingIndex);
            }
        }
    }
}
