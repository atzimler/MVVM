﻿using System.Windows;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests.CompositeComponents
{
    public class ComponentView : IView<IViewModel<ComponentModel>>
    {
        public UIElement UIElement => null;

        public void BindModel(IViewModel<ComponentModel> vm)
        {
        }

        public void UnbindModel()
        {
        }
    }
}
