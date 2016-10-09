using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATZ.MVVM.ViewModels.Utility.Tests.TestHelpers
{
    public class ObservableCollectionEventChangeChecker<T> : ObservableCollection<T>
    {
        public bool CollectionChangedEventHandlerAdded { get; set; }
        public bool CollectionChangedEventHandlerRemoved { get; set; }

        public override event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                base.CollectionChanged += value;
                CollectionChangedEventHandlerAdded = true;
            }
            remove
            {
                base.CollectionChanged -= value;
                CollectionChangedEventHandlerRemoved = true;
            }
        }
    }
}
