using System;
using ATZ.DependencyInjection;
using ATZ.Reflection;

namespace ATZ.MVVM.ViewModels.Utility
{
    /// <summary>
    /// Model ViewModel pair to register them to be bound.
    /// </summary>
    // API v3.2
    public class MvmTuple
    {
        private Type _model;
        private Type _viewModel;

        /// <summary>
        /// The type of the ViewModel as IViewModel&lt;Model&gt;
        /// </summary>
        // API v3.2
        // ReSharper disable once InconsistentNaming => Unfortunately no better name.
        public Type IViewModelTModel { get; private set; }

        /// <summary>
        /// The type of the Model for the MVM pair.
        /// </summary>
        // API v3.2
        public Type Model
        {
            get { return _model; }
            set
            {
                _model = value;
                UpdateCalculatedTypes();
            }
        }

        /// <summary>
        /// The type of the ViewModel for the MVM pair.
        /// </summary>
        // API v3.2
        public Type ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                UpdateCalculatedTypes();
            }
        }

        /// <summary>
        /// Update the calculated types.
        /// </summary>
        // API v3.2
        protected virtual void UpdateCalculatedTypes()
        {
            IViewModelTModel = _model == null ? null : typeof(IViewModel<>).CloseTemplate(new[] { _model });
        }

        /// <summary>
        /// Registers the bindings between the types into the static instance of the Ninject kernel.
        /// </summary>
        /// <exception cref="ArgumentNullException">The Model or the ViewModel is null.</exception>
        // API v3.2
        public virtual void RegisterBindings()
        {
            if (_model == null)
            {
                throw new ArgumentNullException(nameof(Model));
            }

            if (_viewModel == null)
            {
                throw new ArgumentNullException(nameof(ViewModel));
            }

            DependencyResolver.Instance.Bind(IViewModelTModel).To(_viewModel);
        }
}
}
