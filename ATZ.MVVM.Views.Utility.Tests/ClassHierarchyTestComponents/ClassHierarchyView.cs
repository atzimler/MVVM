using System.Windows;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests.ClassHierarchyTestComponents
{
    public class ClassHierarchyView : UserControl, IView<IViewModel<DerivedModel>>, IView<IViewModel<BaseModel>>
    {
        public UIElement UIElement => this;

        // ReSharper disable once UnusedParameter.Local => Demonstration on how to implement combined BindModel.
        private static void BindModelImplementation(IViewModel<BaseModel> vm)
        {
        }

        public void BindModel(IViewModel<DerivedModel> vm) => BindModelImplementation((IViewModel<BaseModel>)vm);
        public void BindModel(IViewModel<BaseModel> vm) => BindModelImplementation(vm);

        public void UnbindModel()
        {
        }
    }
}
