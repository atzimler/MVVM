using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Tests;

namespace ATZ.MVVM.Views.Utility.Tests
{
    public class TestViewModel2 : TestViewModel, IViewModel<TestModel2>
    {
        private new TestModel2 Model { get; set; }

        public new TestModel2 GetModel()
        {
            return Model;
        }

        public void SetModel(TestModel2 model)
        {
            Model = model;
        }
    }
}
