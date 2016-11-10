using System;
using ATZ.MVVM.ViewModels.Utility;

namespace ATZ.MVVM.Views.Utility.Tests.ClassHierarchyTestComponents
{
    public class DerivedViewModel : BaseViewModel<BaseModel>, IViewModel<DerivedModel>
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

        public new DerivedModel GetModel()
        {
            return (DerivedModel) Model;
        }

        public void SetModel(DerivedModel model)
        {
            Model = model;
        }
    }
}
