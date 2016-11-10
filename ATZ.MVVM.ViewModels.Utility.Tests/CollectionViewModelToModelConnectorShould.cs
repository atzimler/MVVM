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
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
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
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };

            var m = new TestModel();
            var vm = new TestViewModel();

            connector.Add(m, vm);

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

            var mc = new ObservableCollectionEventChangeChecker<TestModel>();
            Assert.IsFalse(mc.CollectionChangedEventHandlerAdded);

            connector.ModelCollection = mc;
            Assert.IsTrue(mc.CollectionChangedEventHandlerAdded);
        }

        [Test]
        public void ProperlyUnbindCollectionChanged()
        {
            var mc1 = new ObservableCollectionEventChangeChecker<TestModel>();
            var mc2 = new ObservableCollectionEventChangeChecker<TestModel>();

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
            var mc = new ObservableCollectionEventChangeChecker<TestModel>();
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

        [Test]
        public void BindViewModelIsCalledProperly()
        {
            var binderCalled = false;
            var connector = new TConnector
            {
                BindViewModel = (vm) => { binderCalled = true; },
                ModelCollection = new ObservableCollection<TestModel>(),
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };

            Assert.IsFalse(binderCalled);

            connector.ModelCollection.Add(new TestModel());
            Assert.IsTrue(binderCalled);
        }

        [Test]
        public void ViewModelCollectionIsNotChangedIfSetToTheSame()
        {
            var vmc = new ObservableCollection<IViewModel<TestModel>>();
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel>(),
                ViewModelCollection = vmc
            };

            var collectionChanged = false;
            connector.ModelCollection.Add(new TestModel());
            connector.ViewModelCollection.CollectionChanged += (sender, args) => { collectionChanged = true; };

            connector.ViewModelCollection = vmc;
            Assert.IsFalse(collectionChanged);
        }

        [Test]
        public void NotCrashWhenAddModelWithViewModelIsCalledWithBothCollectionBeingNull()
        {
            var connector = new TConnector();
            Assert.DoesNotThrow(() => connector.Add(new TestModel(), new TestViewModel()));
        }

        [Test]
        public void NotCrashWhenAddModelWithViewModelIsCalledWithNullModelCollection()
        {
            var connector = new TConnector
            {
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };
            Assert.DoesNotThrow(() => connector.Add(new TestModel(), new TestViewModel()));
        }

        [Test]
        public void NotCrashWhenAddModelWithViewModelIsCalledWithNullViewModelCollection()
        {
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel>()
            };
            Assert.DoesNotThrow(() => connector.Add(new TestModel(), new TestViewModel()));
        }

        [Test]
        public void BeAbleToHandleCollectionItemReplace()
        {
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel>(),
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };
            connector.ModelCollection.Add(new TestModel());

            var vm = connector.ViewModelCollection[0];

            connector.ModelCollection[0] = new TestModel();
            Assert.AreNotSame(vm, connector.ViewModelCollection[0]);
        }

        [Test]
        public void BeAbleToHandleCollectionItemRemove()
        {
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel>(),
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };
            connector.ModelCollection.Add(new TestModel());
            Assert.AreEqual(1, connector.ModelCollection.Count);
            Assert.AreEqual(1, connector.ViewModelCollection.Count);

            connector.ModelCollection.RemoveAt(0);
            Assert.AreEqual(0, connector.ModelCollection.Count);
            Assert.AreEqual(0, connector.ViewModelCollection.Count);
        }

        [Test]
        public void BeAbleToHandleCollectionItemReset()
        {
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel>(),
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };
            connector.ModelCollection.Add(new TestModel());
            Assert.AreEqual(1, connector.ModelCollection.Count);
            Assert.AreEqual(1, connector.ViewModelCollection.Count);

            var vm = connector.ViewModelCollection[0];
            var m = new TestModel();
            var mc2 = new ObservableCollection<TestModel> {m};

            connector.ModelCollection = mc2;
            Assert.AreEqual(1, connector.ModelCollection.Count);
            Assert.AreEqual(1, connector.ViewModelCollection.Count);
            Assert.AreNotSame(vm, connector.ViewModelCollection[0]);
        }

        [Test]
        public void BeAbleToHandleCollectionItemMove()
        {
            var m1 = new TestModel();
            var m2 = new TestModel();
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel> {m1, m2},
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };
            Assert.AreEqual(2, connector.ViewModelCollection.Count);
            var vm1 = connector.ViewModelCollection[0];
            var vm2 = connector.ViewModelCollection[1];

            connector.ModelCollection.Move(0, 1);
            Assert.AreSame(vm2, connector.ViewModelCollection[0]);
            Assert.AreSame(vm1, connector.ViewModelCollection[1]);
        }

        [Test]
        public void UnbindViewModelProperly()
        {
            var m = new TestModel();
            var unbindCalled = false;
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel> {m},
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>(),
                UnbindViewModel = vm => unbindCalled = true
            };

            connector.ModelCollection.RemoveAt(0);
            Assert.IsTrue(unbindCalled);
        }

        [Test]
        public void SortCorrectlyIfNoNeedToSwap()
        {
            var m1 = new TestModel();
            var m2 = new TestModel();
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel> {m1, m2},
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };

            connector.Sort((o1, o2) => o1 == m1 && o2 == m2 ? -1 : 1);
            Assert.AreSame(m1, connector.ModelCollection[0]);
            Assert.AreSame(m2, connector.ModelCollection[1]);
        }

        [Test]
        public void SortCorrectlyIfThereIsSwap()
        {
            var m1 = new TestModel();
            var m2 = new TestModel();
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel> {m1, m2},
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };

            connector.Sort((o1, o2) => o1 == m1 && o2 == m2? 1:-1);
            Assert.AreSame(m2, connector.ModelCollection[0]);
            Assert.AreSame(m1, connector.ModelCollection[1]);
        }

        [Test]
        public void ClearAllViewModelBindingsCorrectly()
        {
            var m = new TestModel();
            var unbindCalled = false;
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel> {m},
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>(),
                UnbindViewModel = vm => unbindCalled = true
            };
            Assert.AreEqual(1, connector.ViewModelCollection.Count);

            connector.ClearAllViewModelBindings();
            Assert.AreEqual(1, connector.ViewModelCollection.Count);
            Assert.IsTrue(unbindCalled);
        }

        [Test]
        public void BindViewModelToTheModelProperlyWhenAddingNewModel()
        {
            var m = new TestModel();
            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel>(),
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };
            connector.ModelCollection.Add(m);

            Assert.AreEqual(1, connector.ViewModelCollection.Count);

            var vm = connector.ViewModelCollection[0] as TestViewModel;
            Assert.IsNotNull(vm);
            Assert.IsTrue(vm.BindModelCalled);
        }
    }
}
