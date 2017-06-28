using ATZ.DependencyInjection;
using ATZ.DependencyInjection.System;
using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.ViewModels.Utility.Tests.TestHelpers;
using ATZ.MVVM.Views.Utility.Connectors;
using ATZ.MVVM.Views.Utility.Interfaces;
using ATZ.MVVM.Views.Utility.Tests.ClassHierarchyTestComponents;
using Moq;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Controls;

namespace ATZ.MVVM.Views.Utility.Tests
{
    using TConnector = CollectionViewToViewModelConnector<TestModel>;

    [TestFixture]
    public class CollectionViewToViewModelConnectorShould
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DependencyResolver.Instance.Bind<IView<TestViewModel>>().To<TestView>();
            DependencyResolver.Instance.Bind<IView<TestViewModel2>>().To<TestView2>();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void RetainViewCollection()
        {
            var sp = new StackPanel();
            var v = sp.Children;
            var conn = new TConnector();
            Assert.IsNull(conn.ViewCollection);

            conn.ViewCollection = v;
            Assert.AreSame(v, conn.ViewCollection);
        }

        [Test]
        public void RetainViewModelCollection()
        {
            var vm = new ObservableCollection<IViewModel<TestModel>>();
            var conn = new TConnector();
            Assert.IsNull(conn.ViewModelCollection);

            conn.ViewModelCollection = vm;
            Assert.AreSame(vm, conn.ViewModelCollection);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ProperlyCreateViewForViewModel()
        {
            var stackPanel = new StackPanel();
            var viewCollection = stackPanel.Children;
            Assert.IsNotNull(viewCollection);

            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>>();

            // ReSharper disable once UnusedVariable => Connecting the two collections.
            var conn = new TConnector
            {
                ViewCollection = viewCollection,
                ViewModelCollection = viewModelCollection
            };
            Assert.AreEqual(0, viewCollection.Count);

            var tvm = new TestViewModel();
            viewModelCollection.Add(tvm);

            var view = (TestView)viewCollection[0];
            Assert.IsNotNull(view);
            Assert.AreEqual(1, viewCollection.Count);
            Assert.AreSame(tvm, view.GetViewModel());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void UnbindViewModelCollectionWhenReplaced()
        {
            var sp = new StackPanel();
            var vm = new ObservableCollectionEventChangeChecker<IViewModel<TestModel>>();
            var conn = new TConnector
            {
                ViewCollection = sp.Children,
                ViewModelCollection = vm
            };
            Assert.IsFalse(vm.CollectionChangedEventHandlerRemoved);

            conn.ViewModelCollection = null;
            Assert.IsTrue(vm.CollectionChangedEventHandlerRemoved);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ProperlyMoveViewWhenViewModelIsMoved()
        {
            var vm1 = new TestViewModel();
            var vm2 = new TestViewModel();

            var sp = new StackPanel();
            var viewCollection = sp.Children;
            Assert.IsNotNull(viewCollection);

            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>> { vm1, vm2 };

            // ReSharper disable once UnusedVariable => Connecting the two collections.
            var conn = new TConnector
            {
                ViewCollection = viewCollection,
                ViewModelCollection = viewModelCollection
            };
            Assert.AreEqual(2, viewCollection.Count);

            var v1 = viewCollection[0];
            var v2 = viewCollection[1];

            viewModelCollection.Move(0, 1);
            Assert.AreSame(v2, viewCollection[0]);
            Assert.AreSame(v1, viewCollection[1]);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ProperlyRemoveViewWhenViewModelIsRemoved()
        {
            var sp = new StackPanel();
            var viewCollection = sp.Children;
            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>> { new TestViewModel() };

            Assert.IsNotNull(viewCollection);
            // ReSharper disable once UnusedVariable => Connecting the two collections.
            var conn = new TConnector
            {
                ViewCollection = sp.Children,
                ViewModelCollection = viewModelCollection
            };
            Assert.AreEqual(1, viewCollection.Count);

            viewModelCollection.RemoveAt(0);
            Assert.AreEqual(0, viewCollection.Count);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ProperlyReplaceViewWhenViewModelIsReplaced()
        {
            var sp = new StackPanel();
            var viewCollection = sp.Children;
            Assert.IsNotNull(viewCollection);

            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>> { new TestViewModel() };

            // ReSharper disable once UnusedVariable => Connecting the two collections.
            var conn = new TConnector
            {
                ViewCollection = viewCollection,
                ViewModelCollection = viewModelCollection
            };
            Assert.AreEqual(1, viewCollection.Count);

            var v1 = viewCollection[0];
            viewModelCollection[0] = new TestViewModel();

            Assert.AreEqual(1, viewCollection.Count);
            Assert.AreNotSame(v1, viewCollection[0]);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void KeepViewModelCollectionUnchangedIfReplacedWithTheSameObject()
        {
            var sp = new StackPanel();
            var vms = new ObservableCollectionEventChangeChecker<IViewModel<TestModel>> { new TestViewModel() };
            var conn = new TConnector
            {
                ViewCollection = sp.Children,
                ViewModelCollection = vms
            };
            vms.CollectionChangedEventHandlerAdded = false;
            vms.CollectionChangedEventHandlerRemoved = false;

            conn.ViewModelCollection = vms;
            Assert.IsFalse(vms.CollectionChangedEventHandlerAdded);
            Assert.IsFalse(vms.CollectionChangedEventHandlerRemoved);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void CreateAppropriateTypesDependingOnViewModels()
        {
            var sp = new StackPanel();
            var viewCollection = sp.Children;
            Assert.IsNotNull(viewCollection);

            var vms = new ObservableCollection<IViewModel<TestModel>> { new TestViewModel(), new TestViewModel2() };

            // ReSharper disable once UnusedVariable => Connecting the two collections.
            var conn = new TConnector
            {
                ViewCollection = viewCollection,
                ViewModelCollection = vms
            };

            Assert.AreEqual(2, viewCollection.Count);

            var view1 = viewCollection[0];
            Assert.IsNotNull(view1);
            Assert.AreEqual(typeof(TestView), view1.GetType());

            var view2 = viewCollection[1];
            Assert.IsNotNull(view2);
            Assert.AreEqual(typeof(TestView2), view2.GetType());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void AllowClassHierarchies()
        {
            var view = new ClassHierarchyView();
            var baseView = (IView<IViewModel<BaseModel>>)view;
            Assert.IsNotNull(baseView);

            var derivedView = (IView<IViewModel<DerivedModel>>)view;
            Assert.IsNotNull(derivedView);

            var baseViewModel = baseView.GetViewModel();
            var derivedViewModel = derivedView.GetViewModel();
            Assert.AreSame(baseViewModel, derivedViewModel);

            Assert.IsTrue(typeof(BaseModel).IsAssignableFrom(typeof(DerivedModel)));
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void CreateCorrectViewType()
        {
            var conn = new TConnector();
            var vm = new TestViewModel();
            var view = conn.CreateItem(vm);

            Assert.IsNotNull(view);
            Assert.AreEqual(typeof(TestView), view.GetType());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void WarnIfViewDoesNotImplementCorrectInterface()
        {
            var debug = new Mock<IDebug>();
            debug.Setup(d => d.WriteLine("IView<ATZ.MVVM.ViewModels.Utility.Tests.TestViewModel> was successfully resolved, "
                + "but it has no interface of IView<IViewModel<ATZ.MVVM.ViewModels.Utility.Tests.TestModel>>!"));
            Assert.IsNotNull(debug.Object);

            DependencyResolver.Initialize();
            DependencyInjection.System.Bindings.Initialize();

            DependencyResolver.Instance.Unbind<IDebug>();
            DependencyResolver.Instance.Bind<IDebug>().ToConstant(debug.Object);

            DependencyResolver.Instance.Bind<IView<TestViewModel>>().To<TestViewWithoutInterface>();

            var conn = new TConnector();
            var vm = new TestViewModel();
            var view = conn.CreateItem(vm);

            Assert.IsNull(view);
            debug.VerifyAll();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void BeAbleToConnectToItemCollection()
        {
            var listView = new ListView();
            var viewCollection = listView.Items;
            Assert.IsNotNull(viewCollection);

            var viewModelCollection = new ObservableCollection<IViewModel<TestModel>> { new TestViewModel() };

            // ReSharper disable once UnusedVariable => Connecting the two collections.
            var conn = new TConnector
            {
                ViewCollection = viewCollection,
                ViewModelCollection = viewModelCollection
            };
            Assert.AreEqual(1, viewCollection.Count);

            var view1 = viewCollection[0];
            viewModelCollection[0] = new TestViewModel();

            Assert.AreEqual(1, viewCollection.Count);
            Assert.AreNotSame(view1, viewCollection[0]);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void NotCrashWhenViewCollectionIsNullAndViewModelReplaced()
        {
            var conn = new CollectionViewToViewModelNullifier();
            Assert.AreEqual(1, conn.ViewCollection?.Count);

            Assert.IsNotNull(conn.ViewModelCollection);
            Assert.DoesNotThrow(() => conn.ViewModelCollection[0] = new TestViewModel());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void NotCrashWhenViewCollectionIsNullAndViewModelMoved()
        {
            var conn = new CollectionViewToViewModelNullifier();
            var viewModelCollection = conn.ViewModelCollection;
            Assert.IsNotNull(viewModelCollection);

            viewModelCollection.Add(new TestViewModel());
            Assert.AreEqual(2, viewModelCollection.Count);

            Assert.DoesNotThrow(() => viewModelCollection.Move(0, 1));
        }
    }
}
