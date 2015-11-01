using ViewModels.Utility;
using Views.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Views.Utility.Connectors
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
