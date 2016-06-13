using NUnit.Framework;
using System;
using System.ComponentModel;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    [TestFixture]
    public class BaseViewModelTests
    {
        [Test]
        public void IsValidChangedOnlyFiredWhenRealChangeOccurs()
        {
            TestViewModel vm = new TestViewModel();
            bool fired = false;
            vm.IsValidChanged += (object obj, EventArgs e) => { fired = true; };

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
            TestViewModel vm = new TestViewModel();

            TestModel model = new TestModel();

            Assert.IsFalse(vm.UpdateValidityCalled);

            vm.Model = model;
            Assert.IsTrue(vm.UpdateValidityCalled);
        }

        [Test]
        public void ModelIsUnboundWhenItIsNotNull()
        {
            TestViewModel vm = new TestViewModel();
            TestModel model = new TestModel();
            Assert.IsFalse(vm.UnbindModelCalled);

            vm.Model = model;
            Assert.IsFalse(vm.UnbindModelCalled);

            vm.Model = null;
            Assert.IsTrue(vm.UnbindModelCalled);
        }

        [Test]
        public void OnPropertyChangedOnlyFiredWhenRealChangeOccurs()
        {
            TestViewModel vm = new TestViewModel();
            bool fired = false;
            vm.PropertyChanged += (object obj, PropertyChangedEventArgs e) =>
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
            TestViewModel vm = new TestViewModel();
            Assert.IsFalse(vm.IsValid);

            vm.IsValid = true;

            Assert.IsTrue(vm.IsValid);

            vm.IsValid = false;

            Assert.IsFalse(vm.IsValid);
        }

        [Test]
        public void RetainsModel()
        {
            TestViewModel vm = new TestViewModel();
            TestModel model = new TestModel();

            Assert.IsNull(vm.Model);

            vm.Model = model;
            Assert.AreEqual(model, vm.Model);
        }
    }
}
