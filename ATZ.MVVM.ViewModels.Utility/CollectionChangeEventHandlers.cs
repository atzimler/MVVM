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

        public CollectionChangeEventHandlers(Func<TEventItem, TCollectionItem> create, Action<int, TCollectionItem> insert)
        {
            _create = create;
            _insert = insert;
        }

        public void Add(NotifyCollectionChangedEventArgs e)
        {
            var insertPosition = e.NewStartingIndex;
            foreach (TEventItem model in e.NewItems)
            {
                _insert(insertPosition++, _create(model));
            }
        }
    }
}
