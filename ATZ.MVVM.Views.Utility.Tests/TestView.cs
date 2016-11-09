﻿using System.Windows;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Interfaces;

namespace ATZ.MVVM.Views.Utility.Tests
{
    public class TestView : TextBox, IView<TestViewModel>
    {
        public UIElement UIElement => this;

        public bool BindModelCalled { get; set; }
        public bool UnbindModelCalled { get; set; }

        public void BindModel(TestViewModel vm)
        {
            BindModelCalled = true;
        }

        public void UnbindModel()
        {
            UnbindModelCalled = true;
        }
    }
}
