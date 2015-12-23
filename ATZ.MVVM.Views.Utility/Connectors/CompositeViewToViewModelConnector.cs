using System;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class CompositeViewToViewModelConnector<V, VM, M, CV, CVM, CM>
        where CV : IView<CVM>
        where CVM : BaseViewModel<CM>
        where CM : class
    {
        #region Constructors
        public CompositeViewToViewModelConnector(CV componentView, VM viewModel, Func<VM, CVM> componentViewModel, Func<VM, CM> componentModel)
        {
            CVM cvm = componentViewModel(viewModel);
            cvm.Model = componentModel(viewModel);
            componentView.SetViewModel(cvm);
        }
        #endregion
    }

}
