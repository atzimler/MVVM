using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ATZ.MVVM.Views.Utility.Connectors;
using NUnit.Framework;

namespace ATZ.MVVM.Views.Utility.Tests
{
    [TestFixture]
    public class ObservableCollectionCopierConnectorShould
    {
        private static void AssertCollectionsAreEqual<T>(IEnumerable<T> c1, IEnumerable<T> c2)
        {
            var l1 = c1.ToList();
            var l2 = c2.ToList();

            Assert.AreEqual(l1.Count, l2.Count);
            Assert.AreEqual(CollectionItems(l1), CollectionItems(l2));
        }

        private static string CollectionItems<T>(IEnumerable<T> collection)
        {
            return string.Join(",", collection.ToList().ConvertAll(item => item.ToString()));
        }

        [Test]
        public void AddItemToTheTargetWhenAddedToTheSource()
        {
            var sc = new ObservableCollection<int>();
            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(n => n) {SourceCollection = sc, TargetCollection = tc};
            sc.Add(1);

            AssertCollectionsAreEqual(sc, tc);
        }

        [Test]
        public void ClearTargetCollectionWhenSourceIsCleared()
        {
            var sc = new ObservableCollection<int> {1};
            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(n => n)
            {
                SourceCollection = sc,
                TargetCollection = tc
            };
            AssertCollectionsAreEqual(sc, tc);

            sc.Clear();

            AssertCollectionsAreEqual(sc, tc);
        }

        [Test]
        public void MoveTargetCollectionElementWhenSourceCollectionElementIsMoved()
        {
            var sc = new ObservableCollection<int> {2, 1};
            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(n => n)
            {
                SourceCollection = sc,
                TargetCollection = tc
            };
            AssertCollectionsAreEqual(sc, tc);

            sc.Move(0, 1);

            AssertCollectionsAreEqual(sc, tc);
        }

        [Test]
        public void NotCrashWhenSettingSourceCollection()
        {
            var sc = new ObservableCollection<int> {1};
            var tc = new ObservableCollection<int>();
            var conn = new ObservableCollectionCopierConnector<int, int>(number => number + 1)
            {
                SourceCollection = sc,
                TargetCollection = tc
            };

            Assert.AreSame(sc, conn.SourceCollection);
        }

        [Test]
        public void NotCrashWhenSettingSourceCollectionSecondTime()
        {
            var sc1 = new ObservableCollection<int> {1};
            var sc2 = new ObservableCollection<int> {2};

            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(number => number + 1) {SourceCollection = sc1, TargetCollection = tc};
            conn.SourceCollection = sc2;

            Assert.AreSame(sc2, conn.SourceCollection);
        }

        [Test]
        public void NotRecalculateTargetCollectionIfTheSameTargetCollectionIsSet()
        {
            var sc = new ObservableCollection<int> {1};
            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(number => number + 1)
            {
                SourceCollection = sc,
                TargetCollection = tc
            };

            tc.CollectionChanged +=
                (obj, e) => { Assert.Fail("Target collection items changed when setting to the same collection!"); };

            conn.TargetCollection = tc;
        }

        [Test]
        public void RemoveItemFromTargetWhenRemovedFromSourceCollection()
        {
            var sc = new ObservableCollection<int> {1,2,3};
            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(n => n)
            {
                SourceCollection = sc,
                TargetCollection = tc
            };
            AssertCollectionsAreEqual(sc, tc);

            sc.Remove(2);

            AssertCollectionsAreEqual(sc, tc);
        }

        [Test]
        public void ReplaceItemInTargetWhenReplacedInSourceCollection()
        {
            var sc = new ObservableCollection<int> {1, 2, 3};
            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(n => n)
            {
                SourceCollection = sc,
                TargetCollection = tc
            };
            AssertCollectionsAreEqual(sc, tc);

            sc[0] = 4;

            AssertCollectionsAreEqual(sc, tc);
        }

        [Test]
        public void ProperlyCopyTheObjectsWhenSourceCollectionIsChanged()
        {
            var sc1 = new ObservableCollection<int> {1, 2, 3};
            var sc2 = new ObservableCollection<int> {2, 3, 4};
            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(n => n)
            {
                SourceCollection = sc1,
                TargetCollection = tc
            };
            AssertCollectionsAreEqual(sc1, tc);

            conn.SourceCollection = sc2;

            AssertCollectionsAreEqual(sc2, tc);
        }

        [Test]
        public void RetainTargetCollection()
        {
            var tc = new ObservableCollection<int>();

            var conn = new ObservableCollectionCopierConnector<int, int>(n => n);
            Assert.IsNull(conn.TargetCollection);

            conn.TargetCollection = tc;
            Assert.AreSame(tc, conn.TargetCollection);
        }
    }
}
