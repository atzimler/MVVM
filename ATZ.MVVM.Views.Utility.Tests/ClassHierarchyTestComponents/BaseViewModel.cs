using System;
using ATZ.MVVM.ViewModels.Utility;

namespace ATZ.MVVM.Views.Utility.Tests.ClassHierarchyTestComponents
{
    public class BaseViewModel : BaseViewModel<BaseModel>
    {
        protected override void BindModel()
        {
        }

        protected override void UnbindModel()
        {
        }

        public override void UpdateValidity(object sender, EventArgs e)
        {
        }
    }
}
