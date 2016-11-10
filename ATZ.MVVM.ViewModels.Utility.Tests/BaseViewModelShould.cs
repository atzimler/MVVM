using NUnit.Framework;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    [TestFixture]
    public class BaseViewModelShould
    {
        [Test]
        public void IsValidChangedOnlyFiredWhenRealChangeOccurs()
        {
            IViewModel<TestModel> vm = new TestViewModel();
            var fired = false;
            vm.IsValidChanged += (obj, e) => { fired = true; };

            Assert.IsFalse(vm.IsValid);

            vm.IsValid = false;
            Assert.IsFalse(fired);

            vm.IsValid = true;
            Assert.IsTrue(fired);

            fired = false;
            vm.IsValid = true;
            Assert.IsFalse(fired);
        }

        [Test]
        public void IsValidIsRereadOnModelChange()
        {
            var tvm = new TestViewModel();
            IViewModel<TestModel> vm = tvm;

            var model = new TestModel();
            Assert.IsFalse(tvm.UpdateValidityCalled);

            vm.Model = model;
            Assert.IsTrue(tvm.UpdateValidityCalled);
        }

        [Test]
        public void ModelIsUnboundWhenItIsNotNull()
        {
            var tvm = new TestViewModel();
            IViewModel<TestModel> vm = tvm;
            var model = new TestModel();
            Assert.IsFalse(tvm.UnbindModelCalled);

            vm.Model = model;
            Assert.IsFalse(tvm.UnbindModelCalled);

            vm.Model = null;
            Assert.IsTrue(tvm.UnbindModelCalled);
        }

        [Test]
        public void OnPropertyChangedOnlyFiredWhenRealChangeOccurs()
        {
            var tvm = new TestViewModel();
            IViewModel<TestModel> vm = tvm;
            var fired = false;
            vm.PropertyChanged += (obj, e) =>
            {
                fired = true;
            };

            Assert.IsFalse(vm.IsValid);

            vm.IsValid = false;
            Assert.IsFalse(fired);

            vm.IsValid = true;
            Assert.IsTrue(fired);

            fired = false;
            vm.IsValid = true;
            Assert.IsFalse(fired);
        }

        [Test]
        public void RetainsIsValid()
        {
            IViewModel<TestModel> vm = new TestViewModel();
            Assert.IsFalse(vm.IsValid);

            vm.IsValid = true;

            Assert.IsTrue(vm.IsValid);

            vm.IsValid = false;

            Assert.IsFalse(vm.IsValid);
        }

        [Test]
        public void RetainsModel()
        {
            IViewModel<TestModel> vm = new TestViewModel();
            TestModel model = new TestModel();

            Assert.IsNull(vm.Model);

            vm.Model = model;
            Assert.AreEqual(model, vm.Model);
        }

        [Test]
        public void NotChangeAnythingIfTheSameModelIsAssigned()
        {
            var tvm = new TestViewModel();
            IViewModel<TestModel> vm = tvm;
            var m = new TestModel();

            vm.Model = m;
            Assert.IsFalse(tvm.UnbindModelCalled);

            vm.Model = m;
            Assert.IsFalse(tvm.UnbindModelCalled);
        }

        [Test]
        public void NotCauseNullReferenceExceptionWhenBindingModel()
        {
            // ReSharper disable once ObjectCreationAsStatement => Object construction should not cause exception,
            // so we are not assingning the constructed object to variable.
            Assert.DoesNotThrow(() => new ModelBinderTestViewModel() );
        }
    }
}
