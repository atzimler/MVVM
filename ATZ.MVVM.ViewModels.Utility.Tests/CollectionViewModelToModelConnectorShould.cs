using System.Collections.Generic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using ATZ.MVVM.ViewModels.Utility.Connectors;
using ATZ.MVVM.ViewModels.Utility.Tests.TestHelpers;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    using TConnector = CollectionViewModelToModelConnector<TestViewModel, TestModel>;

    [TestFixture]
    public class CollectionViewModelToModelConnectorShould
    {
        private void CheckValidity(IReadOnlyList<bool> viewModelValidities, bool validity)
        {
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel>(),
                ViewModelCollection = new ObservableCollection<TestViewModel>()
            };

            for (var model = 0; model < viewModelValidities.Count; ++model)
            {
                connector.ModelCollection.Add(new TestModel());
                connector.ViewModelCollection[model].IsValid = viewModelValidities[model];
            }

            Assert.AreEqual(connector.IsValid, validity);
        }

        [Test]
        public void CollectionIsInvalidWhenAnyOfTheEntriesIsInvalid()
        {
            CheckValidity(new[] { false, true }, false);
        }

        [Test]
        public void IsValidChangedOnlyFiredWhenRealChangeOccurs()
        {
            var connector = new TConnector();
            var fired = false;
            connector.IsValidChanged += (obj, e) => { fired = true; };

            Assert.IsFalse(connector.IsValid);

            connector.IsValid = false;
            Assert.IsFalse(fired);

            connector.IsValid = true;
            Assert.IsTrue(fired);

            fired = false;
            connector.IsValid = true;
            Assert.IsFalse(fired);
        }

        [Test]
        public void OnPropertyChangedOnlyFiredWhenRealChangeOccurs()
        {
            var connector = new TConnector();
            var fired = false;
            connector.PropertyChanged += (obj, e) =>
            {
                fired = true;
            };

            Assert.IsFalse(connector.IsValid);

            connector.IsValid = false;
            Assert.IsFalse(fired);

            connector.IsValid = true;
            Assert.IsTrue(fired);

            fired = false;
            connector.IsValid = true;
            Assert.IsFalse(fired);
        }

        [Test]
        public void RetainsIsValid()
        {
            var connector = new TConnector();
            Assert.IsFalse(connector.IsValid);

            connector.IsValid = true;

            Assert.IsTrue(connector.IsValid);

            connector.IsValid = false;

            Assert.IsFalse(connector.IsValid);
        }

        [Test]
        public void ModelAndViewModelCanBeAddedAtTheSameTime()
        {
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel>(),
                ViewModelCollection = new ObservableCollection<TestViewModel>()
            };

            var m = new TestModel();
            var vm = new TestViewModel();

            connector.AddModelWithViewModel(m, vm);

            Assert.AreEqual(1, connector.ModelCollection.Count);
            Assert.AreEqual(1, connector.ViewModelCollection.Count);
            Assert.AreSame(m, connector.ModelCollection[0]);
            Assert.AreSame(vm, connector.ViewModelCollection[0]);
        }

        [Test]
        public void RetainModelCollection()
        {
            var connector = new TConnector();
            Assert.IsNull(connector.ModelCollection);

            var mc = new ObservableCollection<TestModel>();
            connector.ModelCollection = mc;

            Assert.AreSame(mc, connector.ModelCollection);
        }

        [Test]
        public void ProperlyBindCollectionChanged()
        {
            var connector = new TConnector();
            Assert.IsNull(connector.ModelCollection);

            var mc = new ObservableCollectionEventChangeChecker();
            Assert.IsFalse(mc.CollectionChangedEventHandlerAdded);

            connector.ModelCollection = mc;
            Assert.IsTrue(mc.CollectionChangedEventHandlerAdded);
        }

        [Test]
        public void ProperlyUnbindCollectionChanged()
        {
            var mc1 = new ObservableCollectionEventChangeChecker();
            var mc2 = new ObservableCollectionEventChangeChecker();

            var connector = new TConnector { ModelCollection = mc1 };
            mc1.CollectionChangedEventHandlerAdded = false;
            Assert.IsFalse(mc1.CollectionChangedEventHandlerAdded);
            Assert.IsFalse(mc1.CollectionChangedEventHandlerRemoved);
            Assert.IsFalse(mc2.CollectionChangedEventHandlerAdded);
            Assert.IsFalse(mc2.CollectionChangedEventHandlerRemoved);

            connector.ModelCollection = mc2;
            Assert.IsTrue(mc1.CollectionChangedEventHandlerRemoved);
            Assert.IsTrue(mc2.CollectionChangedEventHandlerAdded);
        }

        [Test]
        public void IgnoreChangingTheCollectionIfItIsTheSame()
        {
            var mc = new ObservableCollectionEventChangeChecker();
            var connector = new TConnector
            {
                ModelCollection = mc
            };

            mc.CollectionChangedEventHandlerAdded = false;
            mc.CollectionChangedEventHandlerRemoved = false;

            connector.ModelCollection = mc;
            Assert.IsFalse(mc.CollectionChangedEventHandlerAdded);
            Assert.IsFalse(mc.CollectionChangedEventHandlerRemoved);
        }

        [Test]
        public void NotCrashIfModelCollectionIsSetToNull()
        {
            var mc = new ObservableCollection<TestModel>();
            var connector = new TConnector
            {
                ModelCollection = mc
            };

            Assert.DoesNotThrow(() => connector.ModelCollection = null);
        }
    }
}
