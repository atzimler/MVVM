using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;
using ATZ.Reflection;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ATZ.MVVM.Views.Utility
{
    /// <summary>
    /// Extensions methods to use on IView interfaces.
    /// </summary>
    public static class ViewExtensions
    {
        [NotNull]
        private static readonly Dictionary<object, object> ViewToViewModel = new Dictionary<object, object>();

        [NotNull]
        private static readonly Dictionary<object, ValueTuple<object, Type>> ViewModelToView = new Dictionary<object, ValueTuple<object, Type>>();

        private static void RebindModelOnChange(object sender, [NotNull] PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName) && e.PropertyName != nameof(BaseViewModel<object>.Model))
            {
                return;
            }

            (var view, var modelType) = ViewModelToView[sender];
            var viewModelType = typeof(IViewModel<>).CloseTemplate(new[] { modelType });
            var viewType = typeof(IView<>).CloseTemplate(new[] { viewModelType });
            var unbindModel = viewType.IntrospectionGetMethod(nameof(IView<int>.UnbindModel), new Type[] { });
            var bindModel = viewType.IntrospectionGetMethod(nameof(IView<int>.BindModel), new[] { viewModelType });

            unbindModel?.Invoke(view, new object[] { });
            bindModel?.Invoke(view, new[] { sender });
        }

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

            if (!ViewToViewModel.ContainsKey(view))
            {
                return null;
            }
            return (TViewModel)ViewToViewModel[view];
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
        /// <typeparam name="TModel">The type of the Model.</typeparam>
        /// <param name="view">The View.</param>
        /// <param name="vm">The ViewModel.</param>
        public static void SetViewModel<TModel>(this IView<IViewModel<TModel>> view, IViewModel<TModel> vm)
            where TModel : class
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
                currentViewModel.PropertyChanged -= RebindModelOnChange;
                view.UnbindModel();
            }

            ViewToViewModel[view] = vm;
            ViewModelToView[vm] = (view, typeof(TModel));

            if (vm == null)
            {
                return;
            }

            vm.PropertyChanged += RebindModelOnChange;
            view.BindModel(vm);
        }
    }
}
