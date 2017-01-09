using NUnit.Framework;
using System.ComponentModel;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    [TestFixture]
    public class ObservableObjectShould
    {
        private int _callCounter;

        private void CallCounter(object sender, PropertyChangedEventArgs e)
        {
            _callCounter++;
            Assert.IsNotNull(e);
            Assert.AreEqual("PropertyRaisingChangeNotification", e.PropertyName);
        }

        [SetUp]
        public void SetUp()
        {
            _callCounter = 0;
        }

        [Test]
        public void FireAdditionalPropertyChangedProperly()
        {
            var eventAFired = false;
            var eventBFired = false;

            var vm = new TestViewModel();
            vm.PropertyChanged += (obj, e) =>
            {
                Assert.IsNotNull(e);
                if (e.PropertyName == "A") eventAFired = true;
            };
            vm.PropertyChanged += (obj, e) =>
            {
                Assert.IsNotNull(e);
                if (e.PropertyName == "B") eventBFired = true;
            };

            vm.A++;

            Assert.IsTrue(eventAFired);
            Assert.IsTrue(eventBFired);
        }

        [Test]
        public void FirePropertyChangeNotificationWhenUsingSetWithTwoParameters()
        {
            var eventFired = false;

            var vm = new TestViewModel();
            vm.PropertyChanged += (obj, e) =>
            {
                Assert.IsNotNull(e);
                Assert.AreEqual("SetWith2Parameters", e.PropertyName);
                eventFired = true;
            };

            Assert.AreNotEqual(13, vm.PropertyRaisingChangeNotification);

            vm.SetWith2Parameters(13);

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void NotCrashFromANullableProperty()
        {
            const int value = 13;
            var oo = new ObservableObjectWithPropertyOfType<int?>();
            Assert.DoesNotThrow(() => oo.Property = value);

            Assert.AreEqual(value, oo.Property);
        }

        [Test]
        public void NotCrashFromAStringProperty()
        {
            const string value = "Property";
            var oo = new ObservableObjectWithPropertyOfType<string>();
            Assert.DoesNotThrow(() => oo.Property = value);

            Assert.AreEqual(value, oo.Property);
        }

        [Test]
        public void NotCrashIfAdditionalPropertiesChangedIsNull()
        {
            var vm = new TestViewModel();
            Assert.DoesNotThrow(() => vm.NullAdditionalProperties());
        }

        [Test]
        public void NotFirePropertyChangedWhenValueSetIsTheSame()
        {
            var vm = new TestViewModel();
            vm.PropertyChanged += CallCounter;

            Assert.AreEqual(0, _callCounter);

            var value = vm.PropertyRaisingChangeNotification;
            vm.PropertyRaisingChangeNotification = value;

            Assert.AreEqual(0, _callCounter);
        }

        [Test]
        public void ProperlySuspendPropertyChangedEvent()
        {
            var vm = new TestViewModel();
            vm.PropertyChanged += CallCounter;

            Assert.AreEqual(0, _callCounter);

            using (vm.SuspendPropertyChangedEvent(CallCounter))
            {
                vm.PropertyRaisingChangeNotification++;
                Assert.AreEqual(0, _callCounter);
            }

            vm.PropertyRaisingChangeNotification++;
            Assert.AreEqual(1, _callCounter);
        }
    }
}
