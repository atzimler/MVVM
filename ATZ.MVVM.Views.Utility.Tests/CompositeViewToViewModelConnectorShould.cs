using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Connectors;
using ATZ.MVVM.Views.Utility.Tests.CompositeComponents;
using NUnit.Framework;

namespace ATZ.MVVM.Views.Utility.Tests
{
    using TConnector = CompositeViewToViewModelConnector<MainModel, MainView, ComponentModel, ComponentView>;

    [TestFixture]
    public class CompositeViewToViewModelConnectorShould
    {
        private static ComponentModel ComponentModel(IViewModel<MainModel> vm)
        {
            var mvm = (MainViewModel)vm;
            return mvm?.ComponentModel;
        }

        private static ComponentViewModel ComponentViewModel(IViewModel<MainModel> vm)
        {
            var mvm = (MainViewModel)vm;
            return mvm?.ComponentViewModel;
        }

        [Test]
        public void SetUpCompositeInformationProperly()
        {
            var cvm = new ComponentViewModel();
            var cm = new ComponentModel();
            var mvm = new MainViewModel { ComponentViewModel = cvm, ComponentModel = cm };
            var cv = new ComponentView();

            // ReSharper disable once UnusedVariable => variable needed to correctly create the connector that will set up the MVVM components.
            var conn = new TConnector(cv, mvm, ComponentViewModel, ComponentModel);
            Assert.AreSame(cm, cvm.Model);
            Assert.AreSame(cvm, cv.GetViewModel());
        }

        [Test]
        public void NotCrashIfSomethingIsNull()
        {
            Assert.DoesNotThrow(() =>
            {
                // ReSharper disable once UnusedVariable => variable needed to correctly create the connector that will set up the MVVM components.
                var conn = new TConnector(null, null, vm => null, vm => null);
            });
        }
    }
}
