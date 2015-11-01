using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Utility.Tests
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
    }
}
