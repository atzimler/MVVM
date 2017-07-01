using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Interfaces;
using System.Windows.Controls;

namespace ATZ.MVVM.Views.Utility.Tests
{
    public class TestView2 : ComboBox, IView<IViewModel<TestModel2>>, IView<IViewModel<TestModel>>
    {
        public object UIElement => this;

        // ReSharper disable once UnusedParameter.Local => Just to show how to generalize interface implementation for BindModel.
        private void BindImplementation(IViewModel<TestModel2> vm)
        {
        }

        public void BindModel(IViewModel<TestModel2> vm)
        {
            BindImplementation(vm);
        }

        public void BindModel(IViewModel<TestModel> vm)
        {
            BindImplementation((IViewModel<TestModel2>)vm);
        }

        public void UnbindModel()
        {
        }
    }
}
