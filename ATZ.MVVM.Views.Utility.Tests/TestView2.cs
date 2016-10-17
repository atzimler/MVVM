using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests
{
    public class TestView2 : ComboBox, IView<TestViewModel2>
    {
        public void BindModel(TestViewModel2 vm)
        {
        }

        public void UnbindModel()
        {
        }
    }
}
