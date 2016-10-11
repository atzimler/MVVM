using ATZ.MVVM.Views.Utility.Connectors;
using ATZ.MVVM.Views.Utility.Tests.CompositeComponents;
using NUnit.Framework;

namespace ATZ.MVVM.Views.Utility.Tests
{
    using TConnector = CompositeViewToViewModelConnector<MainView, MainViewModel, MainModel, ComponentView, ComponentViewModel, ComponentModel>;

    [TestFixture]
    class CompositeViewToViewModelConnectorShould
    {
        [Test]
        public void SetUpCompositeInformationProperly()
        {
            var cvm = new ComponentViewModel();
            var cm = new ComponentModel();
            var mvm = new MainViewModel {ComponentViewModel = cvm, ComponentModel = cm};
            var cv = new ComponentView();

            // ReSharper disable once UnusedVariable => variable needed to correctly create the connector that will set up the MVVM components.
            var conn = new TConnector(cv, mvm, vm => vm.ComponentViewModel, vm => vm.ComponentModel);
            Assert.AreSame(cm, cvm.Model);
            Assert.AreSame(cvm, cv.GetViewModel());
        }
    }
}
