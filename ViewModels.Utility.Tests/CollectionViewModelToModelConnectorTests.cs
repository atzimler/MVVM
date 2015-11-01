using ViewModels.Utility.Connectors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Utility.Tests
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using TConnector = CollectionViewModelToModelConnector<TestViewModel, TestModel>;

    [TestFixture]
    public class CollectionViewModelToModelConnectorTests
    {
        #region Private Methods
        private void CheckValidity(bool[] viewModelValidities, bool validity)
        {
            TConnector connector = new TConnector();
            connector.ModelCollection = new ObservableCollection<TestModel>();
            connector.ViewModelCollection = new ObservableCollection<TestViewModel>();

            for (int model = 0; model < viewModelValidities.Count(); ++model)
            {
                connector.ModelCollection.Add(new TestModel());
                connector.ViewModelCollection[model].IsValid = viewModelValidities[model];
            }

            Assert.AreEqual(connector.IsValid, validity);
        }
        #endregion

        #region Public Methods
        [Test]
        public void CollectionIsInvalidWhenAnyOfTheEntriesIsInvalid()
        {
            CheckValidity(new bool[] { false, true }, false);
        }

        [Test]
        public void IsValidChangedOnlyFiredWhenRealChangeOccurs()
        {
            TConnector connector = new TConnector();
            bool fired = false;
            connector.IsValidChanged += (object obj, EventArgs e) => { fired = true; };

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
            TConnector connector = new TConnector();
            bool fired = false;
            connector.PropertyChanged += (object obj, PropertyChangedEventArgs e) =>
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
            TConnector connector = new TConnector();
            Assert.IsFalse(connector.IsValid);

            connector.IsValid = true;

            Assert.IsTrue(connector.IsValid);

            connector.IsValid = false;

            Assert.IsFalse(connector.IsValid);
        }

        #endregion



    
    }
}
