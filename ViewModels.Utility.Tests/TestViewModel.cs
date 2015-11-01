using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Utility.Tests
{
    class TestViewModel : BaseViewModel<TestModel>
    {
        #region Public Properties
        public bool UpdateValidityCalled { get; set; }
        #endregion

        #region Protected Methods
        protected override void UnbindModel()
        {
        }
        #endregion

        #region Public Methods
        public override void UpdateValidity(object sender, EventArgs e)
        {
            UpdateValidityCalled = true;
        }

        public override void BindModel()
        {
        }
        #endregion

    }
}
