using System;
using System.Collections.Generic;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility
{
    public static class ViewExtensions
    {
        private static readonly Dictionary<object, object> Registry = new Dictionary<object,object>();

        public static void ExecuteViewModelMethod<TViewModel>(this IView<TViewModel> view, Action<TViewModel> action)
            where TViewModel : class
        {
            var vm = view.GetViewModel();
            if (vm != null)
            {
                action(vm);
            }
        }

        public static TViewModel GetViewModel<TViewModel>(this object window)
            where TViewModel : class
        {
            if (!Registry.ContainsKey(window))
            {
                return null;
            }
            return (TViewModel)Registry[window];
        }

        public static TViewModel GetViewModel<TViewModel>(this IView<TViewModel> view)
            where TViewModel : class
        {
            if (!Registry.ContainsKey(view))
            {
                return null;
            }
            return (TViewModel)Registry[view];
        }

        public static void SetViewModel<TViewModel>(this IView<TViewModel> view, TViewModel vm)
            where TViewModel : class
        {   
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
