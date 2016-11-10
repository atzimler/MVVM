using System;
using ATZ.MVVM.ViewModels.Utility;

namespace ATZ.MVVM.Views.Utility.Tests.CompositeComponents
{
    public class MainViewModel : BaseViewModel<MainModel>
    {
        public ComponentViewModel ComponentViewModel { get; set; }
        public ComponentModel ComponentModel { get; set; }

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
