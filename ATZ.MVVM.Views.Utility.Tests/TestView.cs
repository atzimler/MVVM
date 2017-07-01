using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Interfaces;
using System.Windows.Controls;

namespace ATZ.MVVM.Views.Utility.Tests
{
    public class TestView : TextBox, IView<IViewModel<TestModel>>
    {
        public object UIElement => this;

        public bool BindModelCalled { get; set; }
        public bool UnbindModelCalled { get; set; }

        public void BindModel(IViewModel<TestModel> vm)
        {
            BindModelCalled = true;
        }

        public void UnbindModel()
        {
            UnbindModelCalled = true;
        }
    }
}
