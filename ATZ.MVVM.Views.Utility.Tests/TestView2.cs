using System.Windows;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests
{
    public class TestView2 : ComboBox, IView<TestViewModel2>, IView<TestViewModel>
    {
        public UIElement UIElement => this;

        // ReSharper disable once UnusedParameter.Local => Just to show how to generalize interface implementation for BindModel.
        private void BindImplementation(TestViewModel vm)
        {
        }

        public void BindModel(TestViewModel2 vm)
        {
            BindImplementation(vm);
        }

        public void BindModel(TestViewModel vm)
        {
            BindImplementation(vm);
        }

        public void UnbindModel()
        {
        }
    }
}
