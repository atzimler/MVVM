using System.Windows;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests
{
    // ReSharper disable once ClassNeverInstantiated.Global => Instantiate by the Ninject system through bindings.
    public class TestViewWithoutInterface : StackPanel, IView<TestViewModel>
    {
        public UIElement UIElement => this;

        public void BindModel(TestViewModel vm)
        {
        }

        public void UnbindModel()
        {
        }
    }
}