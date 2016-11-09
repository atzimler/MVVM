using System;
using ATZ.MVVM.ViewModels.Utility.Connectors;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    using TEntriesConnector = CollectionViewModelToModelConnector<TestViewModel, TestModel>;

    public class ModelBinderTestViewModel : BaseViewModel<ModelBinderTestModel>
    {
        private TEntriesConnector _entriesConnector;

        public ModelBinderTestViewModel()
        {
            Model = new ModelBinderTestModel();
        }

        protected override void BindModel()
        {
            _entriesConnector.ModelCollection = Model.Entries;
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _entriesConnector = new TEntriesConnector();
        }

        protected override void UnbindModel()
        {
        }

        public override void UpdateValidity(object sender, EventArgs e)
        {
        }
    }
}
