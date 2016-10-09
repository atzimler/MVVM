﻿using NUnit.Framework;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    [TestFixture]
    public class BaseViewModelShould
    {
        [Test]
        public void IsValidChangedOnlyFiredWhenRealChangeOccurs()
        {
            var vm = new TestViewModel();
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
            TestViewModel vm = new TestViewModel();

            TestModel model = new TestModel();

            Assert.IsFalse(vm.UpdateValidityCalled);

            vm.Model = model;
            Assert.IsTrue(vm.UpdateValidityCalled);
        }

        [Test]
        public void ModelIsUnboundWhenItIsNotNull()
        {
            var vm = new TestViewModel();
            var model = new TestModel();
            Assert.IsFalse(vm.UnbindModelCalled);

            vm.Model = model;
            Assert.IsFalse(vm.UnbindModelCalled);

            vm.Model = null;
            Assert.IsTrue(vm.UnbindModelCalled);
        }

        [Test]
        public void OnPropertyChangedOnlyFiredWhenRealChangeOccurs()
        {
            var vm = new TestViewModel();
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

        [Test]
        public void NotChangeAnythingIfTheSameModelIsAssigned()
        {
            TestViewModel vm = new TestViewModel();
            TestModel m = new TestModel();

            vm.Model = m;
            Assert.IsFalse(vm.UnbindModelCalled);

            vm.Model = m;
            Assert.IsFalse(vm.UnbindModelCalled);
        }
    }
}