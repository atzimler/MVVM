using System.ComponentModel;
using NUnit.Framework;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    [TestFixture]
    public class ObservableObjectShould
    {
        private int _callCounter;

        private void CallCounter(object sender, PropertyChangedEventArgs e)
        {
            _callCounter++;
            Assert.AreEqual("PropertyRaisingChangeNotification", e.PropertyName);
        }

        [SetUp]
        public void SetUp()
        {
            _callCounter = 0;
        }

        [Test]
        public void FirePropertyChangeNotificationWhenUsingSetWithTwoParameters()
        {
            var eventFired = false;

            var vm = new TestViewModel();
            vm.PropertyChanged += (obj, e) =>
            {
                Assert.AreEqual("SetWith2Parameters", e.PropertyName);
                eventFired = true;
            };

            Assert.AreNotEqual(13, vm.PropertyRaisingChangeNotification);

            vm.SetWith2Parameters(13);

            Assert.IsTrue(eventFired);
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
