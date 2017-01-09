using ATZ.DependencyInjection;
using ATZ.MVVM.ViewModels.Utility.Connectors;
using ATZ.MVVM.ViewModels.Utility.Tests.TestHelpers;
using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    using TConnector = CollectionViewModelToModelConnector<TestModel>;

    [TestFixture]
    public class CollectionViewModelToModelConnectorShould
    {
        private static void CheckValidity([NotNull] IReadOnlyList<bool> viewModelValidities, bool validity)
        {
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };

            for (var model = 0; model < viewModelValidities.Count; ++model)
            {
                modelCollection.Add(new TestModel());
                var modelObject = viewModelCollection[model];

                Assert.IsNotNull(modelObject);
                modelObject.IsValid = viewModelValidities[model];
            }

            Assert.AreEqual(connector.IsValid, validity);
        }

        [SetUp]
        public void SetUp()
        {
            DependencyResolver.Initialize();
            DependencyResolver.Instance.Bind<IViewModel<TestModel>>().To<TestViewModel>();
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
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };

            var m = new TestModel();
            var vm = new TestViewModel();

            connector.Add(m, vm);

            Assert.AreEqual(1, modelCollection.Count);
            Assert.AreEqual(1, viewModelCollection.Count);
            Assert.AreSame(m, modelCollection[0]);
            Assert.AreSame(vm, viewModelCollection[0]);
        }

        [Test]
        public void ModelAndViewModelCanBeAddedAtTheSameTimeWithSimplifiedSyntaxIfTheyAreAlreadyConnected()
        {
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };

            var m = new TestModel();
            var vm = new TestViewModel { Model = m };

            connector.Add(vm);

            Assert.AreEqual(1, modelCollection.Count);
            Assert.AreEqual(1, viewModelCollection.Count);
            Assert.AreSame(m, modelCollection[0]);
            Assert.AreSame(vm, viewModelCollection[0]);
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
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            var binderCalled = false;

            // ReSharper disable once UnusedVariable => The object is going to connect the two observable collections.
            var connector = new TConnector
            {
                BindViewModel = vm => { binderCalled = true; },
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };

            Assert.IsFalse(binderCalled);

            modelCollection.Add(new TestModel());
            Assert.IsTrue(binderCalled);
        }

        [Test]
        public void ViewModelCollectionIsNotChangedIfSetToTheSame()
        {
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };

            var collectionChanged = false;
            modelCollection.Add(new TestModel());

            // This lambda expression should be excluded from the coverage, because its only purpose is verification that it has not been executed.
            viewModelCollection.CollectionChanged += (sender, args) => { collectionChanged = true; };

            connector.ViewModelCollection = viewModelCollection;
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
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            // ReSharper disable once UnusedVariable => The object is going to connect the two observable collections.
            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };
            modelCollection.Add(new TestModel());

            var vm = viewModelCollection[0];

            modelCollection[0] = new TestModel();
            Assert.AreNotSame(vm, viewModelCollection[0]);
        }

        [Test]
        public void BeAbleToHandleCollectionItemRemove()
        {
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            // ReSharper disable once UnusedVariable => The object is going to connect the two observable collections.
            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };
            modelCollection.Add(new TestModel());
            Assert.AreEqual(1, modelCollection.Count);
            Assert.AreEqual(1, viewModelCollection.Count);

            modelCollection.RemoveAt(0);
            Assert.AreEqual(0, modelCollection.Count);
            Assert.AreEqual(0, viewModelCollection.Count);
        }

        [Test]
        public void BeAbleToHandleCollectionItemReset()
        {
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };
            modelCollection.Add(new TestModel());
            Assert.AreEqual(1, modelCollection.Count);
            Assert.AreEqual(1, viewModelCollection.Count);

            var vm = viewModelCollection[0];
            var m = new TestModel();
            var mc2 = new ObservableCollection<TestModel> { m };

            connector.ModelCollection = mc2;
            Assert.AreEqual(1, connector.ModelCollection.Count);
            Assert.AreEqual(1, viewModelCollection.Count);
            Assert.AreNotSame(vm, viewModelCollection[0]);
        }

        [Test]
        public void BeAbleToHandleCollectionItemMove()
        {
            var m1 = new TestModel();
            var m2 = new TestModel();

            var modelCollection = new ObservableCollection<TestModel> { m1, m2 };
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            // ReSharper disable once UnusedVariable => The object is going to connect the two observable collections.
            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };
            Assert.AreEqual(2, viewModelCollection.Count);
            var vm1 = viewModelCollection[0];
            var vm2 = viewModelCollection[1];

            modelCollection.Move(0, 1);
            Assert.AreSame(vm2, viewModelCollection[0]);
            Assert.AreSame(vm1, viewModelCollection[1]);
        }

        [Test]
        public void UnbindViewModelProperly()
        {
            var m = new TestModel();
            var modelCollection = new ObservableCollection<TestModel> { m };


            var unbindCalled = false;
            // ReSharper disable once UnusedVariable => The object is going to connect the two observable collections.
            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>(),
                UnbindViewModel = vm => unbindCalled = true
            };

            modelCollection.RemoveAt(0);
            Assert.IsTrue(unbindCalled);
        }

        [Test]
        public void SortCorrectlyIfNoNeedToSwap()
        {
            var m1 = new TestModel();
            var m2 = new TestModel();
            var modelCollection = new ObservableCollection<TestModel> { m1, m2 };

            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };

            connector.Sort((o1, o2) => o1 == m1 && o2 == m2 ? -1 : 1);
            Assert.AreSame(m1, modelCollection[0]);
            Assert.AreSame(m2, modelCollection[1]);
        }

        [Test]
        public void SortCorrectlyIfThereIsSwap()
        {
            var m1 = new TestModel();
            var m2 = new TestModel();
            var modelCollection = new ObservableCollection<TestModel> { m1, m2 };

            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = new ObservableCollection<IViewModel<TestModel>>()
            };

            connector.Sort((o1, o2) => o1 == m1 && o2 == m2 ? 1 : -1);
            Assert.AreSame(m2, modelCollection[0]);
            Assert.AreSame(m1, modelCollection[1]);
        }

        [Test]
        public void ClearAllViewModelBindingsCorrectly()
        {
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            var m = new TestModel();
            var unbindCalled = false;

            var connector = new TConnector
            {
                ModelCollection = new ObservableCollection<TestModel> { m },
                ViewModelCollection = viewModelCollection,
                UnbindViewModel = vm => unbindCalled = true
            };
            Assert.AreEqual(1, viewModelCollection.Count);

            connector.ClearAllViewModelBindings();
            Assert.AreEqual(1, viewModelCollection.Count);
            Assert.IsTrue(unbindCalled);
        }

        [Test]
        public void BindViewModelToTheModelProperlyWhenAddingNewModel()
        {
            var m = new TestModel();
            var modelCollection = new ObservableCollection<TestModel>();
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            // ReSharper disable once UnusedVariable => The object is going to connect the two observable collections.
            var connector = new TConnector
            {
                ModelCollection = modelCollection,
                ViewModelCollection = viewModelCollection
            };
            modelCollection.Add(m);

            Assert.AreEqual(1, viewModelCollection.Count);

            var vm = viewModelCollection[0] as TestViewModel;
            Assert.IsNotNull(vm);
            Assert.IsTrue(vm.BindModelCalled);
        }
    }
}
