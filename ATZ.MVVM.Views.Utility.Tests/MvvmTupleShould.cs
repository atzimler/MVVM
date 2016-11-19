using System;
using ATZ.DependencyInjection;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.Views.Utility.Interfaces;
using NUnit.Framework;

namespace ATZ.MVVM.Views.Utility.Tests
{
    [TestFixture]
    public class MvvmTupleShould
    {
        [SetUp]
        public void SetUp()
        {
            DependencyResolver.Initialize();
        }

        [Test]
        public void AllowUninitializedTuple()
        {
            // ReSharper disable once ObjectCreationAsStatement => Testing constructor.
            Assert.DoesNotThrow(() => new MvvmTuple());
        }

        [Test]
        public void RetainView()
        {
            var tuple = new MvvmTuple {View = typeof(double)};
            Assert.AreEqual(typeof(double), tuple.View);
        }

        [Test]
        public void AllowModelToBeSetToNull()
        {
            var tuple = new MvvmTuple();
            Assert.DoesNotThrow(() => tuple.Model = null);
        }

        [Test]
        public void AllowViewModelToBeSetToNull()
        {
            var tuple = new MvvmTuple();
            Assert.DoesNotThrow(() => tuple.ViewModel = null);
        }

        [Test]
        public void ProperlyCalculateViewModelInterface()
        {
            var tuple = new MvvmTuple {ViewModel = typeof(TestViewModel)};
            Assert.AreEqual(typeof(IView<TestViewModel>), tuple.IViewTViewModel);
        }

        [Test]
        public void ProperlyCalculateModelInterface()
        {
            var tuple = new MvvmTuple {Model = typeof(TestModel)};
            Assert.AreEqual(typeof(IView<IViewModel<TestModel>>), tuple.IViewTIViewModel);
        }

        [Test]
        public void ThrowExceptionWhenTryingToRegisterNullView()
        {
            var tuple = new MvvmTuple {View = null};
            var ex = Assert.Throws<ArgumentNullException>(() => tuple.RegisterBindings());
            Assert.AreEqual("View", ex.ParamName);
        }

        [Test]
        public void TolerateSettingTheModelToNull()
        {
            var tuple = new MvvmTuple();
            Assert.DoesNotThrow(() => tuple.Model = null);
        }

        [Test]
        public void ProperlyDetermineViewModelInterfaceType()
        {
            var tuple = new MvvmTuple { Model = typeof(int) };
            Assert.AreEqual(typeof(IViewModel<int>), tuple.IViewModelTModel);
        }

        [Test]
        public void RetainModel()
        {
            var tuple = new MvvmTuple { Model = typeof(int) };
            Assert.AreEqual(typeof(int), tuple.Model);
        }

        [Test]
        public void RetainViewModel()
        {
            var tuple = new MvvmTuple { ViewModel = typeof(double) };
            Assert.AreEqual(typeof(double), tuple.ViewModel);
        }

        [Test]
        public void ThrowExceptionWhenTryingToRegisterNullModel()
        {
            var tuple = new MvvmTuple { Model = null, View = typeof(TestView), ViewModel = typeof(TestViewModel)};
            var ex = Assert.Throws<ArgumentNullException>(() => tuple.RegisterBindings());
            Assert.AreEqual("Model", ex.ParamName);
        }

        [Test]
        public void ThrowExceptionWhenTryingToRegisterNullViewModel()
        {
            var tuple = new MvvmTuple { Model = typeof(int), ViewModel = null, View = typeof(TestView) };
            var ex = Assert.Throws<ArgumentNullException>(() => tuple.RegisterBindings());
            Assert.AreEqual("ViewModel", ex.ParamName);
        }

        [Test]
        // TODO: Update NUnit used by the project from v2 to v3.
        [STAThread]
        public void ProperlyRegisterBindings()
        {
            var tuple = new MvvmTuple { Model = typeof(TestModel), ViewModel = typeof(TestViewModel), View = typeof(TestView) };
            tuple.RegisterBindings();

            BindingVerification.VerifyBinding<IViewModel<TestModel>, TestViewModel>();
            BindingVerification.VerifyBinding<IView<TestViewModel>, TestView>();
            BindingVerification.VerifyBinding<IView<IViewModel<TestModel>>, TestView>();
        }


    }
}
