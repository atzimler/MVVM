using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests.CompositeComponents
{
    public class ComponentView : IView<ComponentViewModel>
    {
        public void BindModel(ComponentViewModel vm)
        {
        }

        public void UnbindModel()
        {
        }
    }
}
