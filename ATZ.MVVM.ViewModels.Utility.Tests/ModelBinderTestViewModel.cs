using ATZ.MVVM.ViewModels.Utility.Connectors;
using NUnit.Framework;
using System;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    using TEntriesConnector = CollectionViewModelToModelConnector<TestModel>;

    public class ModelBinderTestViewModel : BaseViewModel<ModelBinderTestModel>
    {
        private TEntriesConnector _entriesConnector;

        public ModelBinderTestViewModel()
        {
            Model = new ModelBinderTestModel();
        }

        protected override void BindModel()
        {
            Assert.IsNotNull(_entriesConnector);
            _entriesConnector.ModelCollection = Model?.Entries;
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _entriesConnector = new TEntriesConnector();
        }

        protected override void UnbindModel()
        {
            Assert.IsNotNull(_entriesConnector);
            _entriesConnector.ModelCollection = null;
        }

        public override void UpdateValidity(object sender, EventArgs e)
        {
        }
    }
}
