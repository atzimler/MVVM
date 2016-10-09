using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests
{
    class TestView : TextBox, IView<TestViewModel>
    {
        public bool BindModelCalled { get; set; }
        public bool UnbindModelCalled { get; set; }

        public void BindModel(TestViewModel vm)
        {
            BindModelCalled = true;
        }

        public void UnbindModel()
        {
            UnbindModelCalled = true;
        }
    }
}
