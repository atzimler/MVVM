﻿using JetBrains.Annotations;

namespace ATZ.MVVM.Views.Utility.Interfaces
{
    /// <summary>
    /// Interface to implement Views connected to ViewModels while not deriving from an abstract View class as they need to contain user interface elements from the GUI system
    /// and multiple inheritance is not supported.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public interface IView<in TViewModel>
    {
        /// <summary>
        /// The UIElement object of the View.
        /// </summary>
        // ReSharper disable once InconsistentNaming => Name of the type is spelled correctly as UIElement
        object UIElement { get; }

        /// <summary>
        /// Bind the ViewModel to the View.
        /// </summary>
        /// <param name="vm">The ViewModel to bind to.</param>
        void BindModel([NotNull] TViewModel vm);

        /// <summary>
        /// Unbind the ViewModel from the View.
        /// </summary>
        void UnbindModel();
    }
}
