using System;
using ATZ.MVVM.ViewModels.Utility.Tests;
using NUnit.Core;
using NUnit.Framework;

namespace ATZ.MVVM.Views.Utility.Tests
{
    [TestFixture]
    class ViewExtensionShould
    {
        [Test]
        [STAThread]
        public void NotBindViewModelWhenTheSameModelIsSet()
        {
            var vm = new TestViewModel();
            var v = new TestView();

            v.SetViewModel(vm);
            v.UnbindModelCalled = false;
            Assert.IsFalse(v.UnbindModelCalled);

            v.SetViewModel(vm);
            Assert.IsFalse(v.UnbindModelCalled);
        }

        [Test]
        [STAThread]
        public void UnbindPreviousViewProperly()
        {
            var vm1 = new TestViewModel();
            var vm2 = new TestViewModel();
            var v = new TestView();

            v.SetViewModel(vm1);
            v.UnbindModelCalled = false;
            Assert.IsFalse(v.UnbindModelCalled);

            v.SetViewModel(vm2);
            Assert.IsTrue(v.UnbindModelCalled);
        }

        [Test]
        [STAThread]
        public void ReturnNullIfViewIsNotRegistered()
        {
            var v = new TestView();
            var o = (object) v;
            Assert.IsNull(o.GetViewModel<TestViewModel>());
        }

        [Test]
        [STAThread]
        public void ReturnProperViewModelIfViewIsRegistered()
        {
            var v = new TestView();
            var vm = new TestViewModel();
            var o = (object) v;
            v.SetViewModel(vm);

            Assert.AreSame(vm, o.GetViewModel<TestViewModel>());
        }

        [Test]
        [STAThread]
        public void NotCrashIfViewModelIsNotSetAndExecuteActionIsCalled()
        {
            var v = new TestView();
            Assert.DoesNotThrow(() => v.ExecuteViewModelMethod(vm => vm.Execute()));
        }

        [Test]
        [STAThread]
        public void ProperlyExecuteActionIfViewModelIsSet()
        {
            var v = new TestView();
            var tvm = new TestViewModel();
            v.SetViewModel(tvm);
            v.ExecuteViewModelMethod(vm => vm.Execute());
            Assert.IsTrue(tvm.ExecuteCalled);
        }

        [Test]
        [STAThread]
        public void CallBindModelOnView()
        {
            var v = new TestView();
            var tvm = new TestViewModel();
            v.SetViewModel(tvm);
            Assert.IsTrue(v.BindModelCalled);
        }
    }
}
