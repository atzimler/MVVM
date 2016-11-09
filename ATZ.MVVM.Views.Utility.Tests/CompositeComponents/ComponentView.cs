using System.Windows;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests.CompositeComponents
{
    public class ComponentView : IView<ComponentViewModel>
    {
        public UIElement UIElement => null;

        public void BindModel(ComponentViewModel vm)
        {
        }

        public void UnbindModel()
        {
        }
    }
}
