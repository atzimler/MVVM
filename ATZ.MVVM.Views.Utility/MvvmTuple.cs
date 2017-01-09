using ATZ.DependencyInjection;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;
using ATZ.Reflection;
using System;

namespace ATZ.MVVM.Views.Utility
{
    /// <summary>
    /// Model View ViewModel triplet to register them to be bound.
    /// </summary>
    // API v3.2
    public class MvvmTuple : MvmTuple
    {
        /// <summary>
        /// The type of the View as IView&lt;IViewModel&lt;Model&gt;&gt;
        /// </summary>
        // API v3.2
        // ReSharper disable once InconsistentNaming => Unfortunately no better name.
        public Type IViewTIViewModel { get; private set; }

        /// <summary>
        /// The type of the View as IView&lt;ViewModel&gt;
        /// </summary>
        // API v3.2
        // ReSharper disable once InconsistentNaming => Unfortunately no better name.
        public Type IViewTViewModel { get; private set; }

        /// <summary>
        /// The type of the View.
        /// </summary>
        // API v3.2
        public Type View { get; set; }

        /// <summary>
        /// Update the calculated types.
        /// </summary>
        // API v3.2
        protected override void UpdateCalculatedTypes()
        {
            base.UpdateCalculatedTypes();

            IViewTViewModel = ViewModel == null ? null : typeof(IView<>).CloseTemplate(new[] { ViewModel });
            IViewTIViewModel = IViewModelTModel == null ? null : typeof(IView<>).CloseTemplate(new[] { IViewModelTModel });
        }

        /// <summary>
        /// Registers the bindings between the types into the static instance of the Ninject kernel.
        /// </summary>
        /// <exception cref="InvalidOperationException">Any of the Model, View or the ViewModel types is null.</exception>
        // API v3.2
        public override void RegisterBindings()
        {
            var view = View;
            if (view == null)
            {
                throw new InvalidOperationException("Trying to register an MvvmTupple while MvvmTuple.View == null!");
            }

            base.RegisterBindings();

            DependencyResolver.Instance.Bind(IViewTViewModel).To(view);
            DependencyResolver.Instance.Bind(IViewTIViewModel).To(view);
        }
    }
}
