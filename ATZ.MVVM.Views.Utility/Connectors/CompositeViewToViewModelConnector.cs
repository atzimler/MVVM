using System;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    /// <summary>
    /// Associate a part of the MVVM object with a sub MVVM structure.
    /// For example an MVVM structure of the button (component object) as part of an MVVM structure of the Window (main object).
    /// </summary>
    /// <typeparam name="TModel">The Model of the main object.</typeparam>
    /// <typeparam name="TView">The View of the main object.</typeparam>
    /// <typeparam name="TViewModel">The ViewModel of the main object.</typeparam>
    /// <typeparam name="TComponentModel">The Model of the component object.</typeparam>
    /// <typeparam name="TComponentView">The View of the component object.</typeparam>
    /// <typeparam name="TComponentViewModel">The ViewModel of the component object.</typeparam>
    // ReSharper disable UnusedTypeParameter => To simplify usage, we require the MVVM types in pair.
    public class CompositeViewToViewModelConnector<TModel, TView, TViewModel, TComponentModel, TComponentView, TComponentViewModel>
    // ReSharper restore UnusedTypeParameter
        where TComponentView : IView<TComponentViewModel>
        where TComponentViewModel : BaseViewModel<TComponentModel>
        where TComponentModel : class
    {
        /// <summary>
        /// Set up the component MVVM structure into the main MVVM structure.
        /// </summary>
        /// <param name="componentView">The View of the component.</param>
        /// <param name="viewModel">The ViewModel of the main object.</param>
        /// <param name="componentViewModel">Function to extract the ViewModel of the component from the ViewModel of the main object.</param>
        /// <param name="componentModel">Function to extract the Model of the component from the Model of the main object.</param>
        public CompositeViewToViewModelConnector(TComponentView componentView, TViewModel viewModel, Func<TViewModel, TComponentViewModel> componentViewModel, Func<TViewModel, TComponentModel> componentModel)
        {
            var cvm = componentViewModel(viewModel);
            cvm.Model = componentModel(viewModel);
            componentView.SetViewModel(cvm);
        }
    }

}
