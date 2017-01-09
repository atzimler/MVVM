using ATZ.MVVM.Views.Utility.Interfaces;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace ATZ.MVVM.Views.Utility
{
    /// <summary>
    /// Extensions methods to use on IView interfaces.
    /// </summary>
    public static class ViewExtensions
    {
        [NotNull]
        private static readonly Dictionary<object, object> Registry = new Dictionary<object, object>();

        /// <summary>
        /// Execute a method on the ViewModel associated with the View.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="view">The view.</param>
        /// <param name="action">The action to execute, usually a method call on the ViewModel.</param>
        public static void ExecuteViewModelMethod<TViewModel>(this IView<TViewModel> view, [NotNull] Action<TViewModel> action)
            where TViewModel : class
        {
            var vm = view.GetViewModel();
            if (vm != null)
            {
                action(vm);
            }
        }

        /// <summary>
        /// Get the ViewModel associated with the View.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel</typeparam>
        /// <param name="view">The View.</param>
        /// <returns>The ViewModel associated with the View.</returns>
        public static TViewModel GetViewModel<TViewModel>(this object view)
            where TViewModel : class
        {
            if (view == null)
            {
                return null;
            }

            if (!Registry.ContainsKey(view))
            {
                return null;
            }
            return (TViewModel)Registry[view];
        }

        /// <summary>
        /// Get the ViewModel associated with the View.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel</typeparam>
        /// <param name="view">The View.</param>
        /// <returns>The ViewModel associated with the View.</returns>
        public static TViewModel GetViewModel<TViewModel>(this IView<TViewModel> view)
            where TViewModel : class
        {
            return GetViewModel<TViewModel>((object)view);
        }

        /// <summary>
        /// Associate the ViewModel with the View.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel.</typeparam>
        /// <param name="view">The View.</param>
        /// <param name="vm">The ViewModel.</param>
        public static void SetViewModel<TViewModel>(this IView<TViewModel> view, TViewModel vm)
            where TViewModel : class
        {
            if (view == null)
            {
                return;
            }

            var currentViewModel = view.GetViewModel();
            if (currentViewModel == vm)
            {
                return;
            }

            if (currentViewModel != null)
            {
                view.UnbindModel();
            }

            Registry[view] = vm;

            if (vm != null)
            {
                view.BindModel(vm);
            }
        }
    }
}
