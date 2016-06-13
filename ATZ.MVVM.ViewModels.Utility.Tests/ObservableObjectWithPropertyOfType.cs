using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    public class ObservableObjectWithPropertyOfType<T> : ObservableObject
    {
        private T _property;

        public T Property
        {
            get { return _property; }
            set { Set(ref _property, value); }
        }
    }
}
