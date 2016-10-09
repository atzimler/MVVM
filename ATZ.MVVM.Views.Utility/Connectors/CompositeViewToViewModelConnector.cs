using System;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    public class CompositeViewToViewModelConnector<TView, TViewModel, TModel, TComponentView, TComponentViewModel, TComponentModel>
        where TComponentView : IView<TComponentViewModel>
        where TComponentViewModel : BaseViewModel<TComponentModel>
        where TComponentModel : class
    {
        #region Constructors
        public CompositeViewToViewModelConnector(TComponentView componentView, TViewModel viewModel, Func<TViewModel, TComponentViewModel> componentViewModel, Func<TViewModel, TComponentModel> componentModel)
        {
            TComponentViewModel cvm = componentViewModel(viewModel);
            cvm.Model = componentModel(viewModel);
            componentView.SetViewModel(cvm);
        }
        #endregion
    }

}
