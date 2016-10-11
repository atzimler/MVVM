using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ATZ.MVVM.ViewModels.Utility.Tests;
using ATZ.MVVM.ViewModels.Utility.Tests.TestHelpers;
using ATZ.MVVM.Views.Utility.Connectors;
using NUnit.Framework;

namespace ATZ.MVVM.Views.Utility.Tests
{
    using TConnector = CollectionViewToViewModelConnector<TestView, TestViewModel, TestModel>;

    [TestFixture]
    public class CollectionViewToViewModelConnectorShould
    {
        [Test]
        [STAThread]
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
            var vm = new ObservableCollection<TestViewModel>();
            var conn = new TConnector();
            Assert.IsNull(conn.ViewModelCollection);

            conn.ViewModelCollection = vm;
            Assert.AreSame(vm, conn.ViewModelCollection);
        }

        [Test]
        [STAThread]
        public void ProperlyCreateViewForViewModel()
        {
            var sp = new StackPanel();
            var vm = new ObservableCollection<TestViewModel>();
            var conn = new TConnector
            {
                ViewCollection = sp.Children,
                ViewModelCollection = vm
            };
            Assert.AreEqual(0, sp.Children.Count);

            var tvm = new TestViewModel();
            conn.ViewModelCollection.Add(tvm);
            Assert.AreEqual(1, sp.Children.Count);
            Assert.AreSame(tvm, ((TestView)sp.Children[0]).GetViewModel());
        }

        [Test]
        [STAThread]
        public void UnbindViewModelCollectionWhenReplaced()
        {
            var sp = new StackPanel();
            var vm = new ObservableCollectionEventChangeChecker<TestViewModel>();
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
        [STAThread]
        public void ProperlyMoveViewWhenViewModelIsMoved()
        {
            var vm1 = new TestViewModel();
            var vm2 = new TestViewModel();

            var sp = new StackPanel();
            var vms = new ObservableCollection<TestViewModel> {vm1, vm2};
            var conn = new TConnector
            {
                ViewCollection = sp.Children,
                ViewModelCollection = vms
            };
            Assert.AreEqual(2, conn.ViewCollection.Count);

            var v1 = sp.Children[0];
            var v2 = sp.Children[1];

            vms.Move(0, 1);
            Assert.AreSame(v2, sp.Children[0]);
            Assert.AreSame(v1, sp.Children[1]);
        }

        [Test]
        [STAThread]
        public void ProperlyRemoveViewWhenViewModelIsRemoved()
        {
            var sp = new StackPanel();
            var vms = new ObservableCollection<TestViewModel> {new TestViewModel()};
            var conn = new TConnector
            {
                ViewCollection = sp.Children,
                ViewModelCollection = vms
            };
            Assert.AreEqual(1, conn.ViewCollection.Count);

            vms.RemoveAt(0);
            Assert.AreEqual(0, sp.Children.Count);
        }

        [Test]
        [STAThread]
        public void ProperlyReplaceViewWhenViewModelIsReplaced()
        {
            var sp = new StackPanel();
            var vms = new ObservableCollection<TestViewModel> {new TestViewModel()};
            var conn = new TConnector
            {
                ViewCollection = sp.Children,
                ViewModelCollection = vms
            };
            Assert.AreEqual(1, sp.Children.Count);

            var v1 = sp.Children[0];
            conn.ViewModelCollection[0] = new TestViewModel();
            Assert.AreEqual(1, sp.Children.Count);
            Assert.AreNotSame(v1, sp.Children[0]);
        }

        [Test]
        [STAThread]
        public void KeepViewModelCollectionUnchangedIfReplacedWithTheSameObject()
        {
            var sp = new StackPanel();
            var vms = new ObservableCollectionEventChangeChecker<TestViewModel> {new TestViewModel()};
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
    }
}
