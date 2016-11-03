using System;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    // ReSharper disable UnusedTypeParameter => To simplify usage, we require the MVVM types in pair.
    public class CompositeViewToViewModelConnector<TModel, TView, TViewModel, TComponentModel, TComponentView, TComponentViewModel>
    // ReSharper restore UnusedTypeParameter
        where TComponentView : IView<TComponentViewModel>
        where TComponentViewModel : BaseViewModel<TComponentModel>
        where TComponentModel : class
    {
        public CompositeViewToViewModelConnector(TComponentView componentView, TViewModel viewModel, Func<TViewModel, TComponentViewModel> componentViewModel, Func<TViewModel, TComponentModel> componentModel)
        {
            var cvm = componentViewModel(viewModel);
            cvm.Model = componentModel(viewModel);
            componentView.SetViewModel(cvm);
        }
    }

}
