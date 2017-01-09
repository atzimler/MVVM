using ATZ.DependencyInjection;
using NUnit.Framework;
using System;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    [TestFixture]
    public class MvmTupleShould
    {
        [SetUp]
        public void SetUp()
        {
            DependencyResolver.Initialize();
        }

        [Test]
        public void TolerateNotSettingAnyOfTheProperties()
        {
            // ReSharper disable once ObjectCreationAsStatement => testing of the constructor.
            Assert.DoesNotThrow(() => new MvmTuple());
        }

        [Test]
        public void TolerateSettingTheModelToNull()
        {
            var tuple = new MvmTuple();
            Assert.DoesNotThrow(() => tuple.Model = null);
        }

        [Test]
        public void ProperlyDetermineViewModelInterfaceType()
        {
            var tuple = new MvmTuple { Model = typeof(int) };
            Assert.AreEqual(typeof(IViewModel<int>), tuple.IViewModelTModel);
        }

        [Test]
        public void RetainModel()
        {
            var tuple = new MvmTuple { Model = typeof(int) };
            Assert.AreEqual(typeof(int), tuple.Model);
        }

        [Test]
        public void RetainViewModel()
        {
            var tuple = new MvmTuple { ViewModel = typeof(double) };
            Assert.AreEqual(typeof(double), tuple.ViewModel);
        }

        [Test]
        public void ThrowExceptionWhenTryingToRegisterNullModel()
        {
            var tuple = new MvmTuple { Model = null };
            var ex = Assert.Throws<ArgumentNullException>(() => tuple.RegisterBindings());
            Assert.IsNotNull(ex);
            Assert.AreEqual("Model", ex.ParamName);
        }

        [Test]
        public void ThrowExceptionWhenTryingToRegisterNullViewModel()
        {
            var tuple = new MvmTuple { Model = typeof(int), ViewModel = null };
            var ex = Assert.Throws<ArgumentNullException>(() => tuple.RegisterBindings());
            Assert.IsNotNull(ex);
            Assert.AreEqual("ViewModel", ex.ParamName);
        }

        [Test]
        public void ProperlyRegisterBindings()
        {
            var tuple = new MvmTuple { Model = typeof(TestModel), ViewModel = typeof(TestViewModel) };
            tuple.RegisterBindings();

            BindingVerification.VerifyBinding<IViewModel<TestModel>, TestViewModel>();
        }
    }
}
