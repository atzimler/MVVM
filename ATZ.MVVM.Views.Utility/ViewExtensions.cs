using System;
using System.Collections.Generic;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility
{
    public static class ViewExtensions
    {
        #region Private Variables
        private static Dictionary<object, object> _registry = new Dictionary<object,object>();
        #endregion

        #region Public Methods
        public static void ExecuteViewModelMethod<VM>(this IView<VM> view, Action<VM> action)
            where VM : class
        {
            VM vm = view.GetViewModel();
            if (vm != null)
            {
                action(vm);
            }
        }

        public static VM GetViewModel<VM>(this object window)
            where VM : class
        {
            if (!_registry.ContainsKey(window))
            {
                return null;
            }
            return (VM)_registry[window];
        }

        public static VM GetViewModel<VM>(this IView<VM> view)
            where VM : class
        {
            if (!_registry.ContainsKey(view))
            {
                return null;
            }
            return (VM)_registry[view];
        }

        public static void SetViewModel<VM>(this IView<VM> view, VM vm)
            where VM : class
        {   
            VM currentViewModel = view.GetViewModel();
            if (currentViewModel == vm)
            {
                return;
            }

            if (currentViewModel != null)
            {
                view.UnbindModel();
            }

            _registry[view] = vm;

            if (vm != null)
            {
                view.BindModel(vm);
            }
        }
        #endregion
    }
}
