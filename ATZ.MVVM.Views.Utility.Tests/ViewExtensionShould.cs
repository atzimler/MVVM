using System.Threading;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Interfaces;
using NUnit.Framework;

namespace ATZ.MVVM.Views.Utility.Tests
{
    [TestFixture]
    public class ViewExtensionShould
    {
        [Test]
        [Apartment(ApartmentState.STA)]
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
        [Apartment(ApartmentState.STA)]
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
        [Apartment(ApartmentState.STA)]
        public void ReturnNullIfViewIsNotRegistered()
        {
            var v = new TestView();
            var o = (object) v;
            Assert.IsNull(o.GetViewModel<TestViewModel>());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ReturnProperViewModelIfViewIsRegistered()
        {
            var v = new TestView();
            var vm = new TestViewModel();
            var o = (object) v;
            v.SetViewModel(vm);

            Assert.AreSame(vm, o.GetViewModel<TestViewModel>());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void NotCrashIfViewModelIsNotSetAndExecuteActionIsCalled()
        {
            var v = new TestView();
            Assert.DoesNotThrow(() => v.ExecuteViewModelMethod(vm => ((TestViewModel)vm).Execute()));
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ProperlyExecuteActionIfViewModelIsSet()
        {
            var v = new TestView();
            var tvm = new TestViewModel();
            v.SetViewModel(tvm);
            v.ExecuteViewModelMethod(vm => ((TestViewModel)vm).Execute());
            Assert.IsTrue(tvm.ExecuteCalled);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void CallBindModelOnView()
        {
            var v = new TestView();
            var tvm = new TestViewModel();
            v.SetViewModel(tvm);
            Assert.IsTrue(v.BindModelCalled);
        }

        [Test]
        public void ReturnNullForNullViewsViewModel()
        {
            IView<TestViewModel> vm = null;
            Assert.IsNull(vm.GetViewModel());
        }

        [Test]
        public void NotCrashWhenTryingToSetViewModelOnANullView()
        {
            IView<TestViewModel> vm = null;
            // ReSharper disable once ExpressionIsAlwaysNull => Yes, that is what we are testing.
            Assert.DoesNotThrow(() => vm.SetViewModel(new TestViewModel()));
        }
    }
}
