using System.ComponentModel;
using NUnit.Framework;
using ATZ.MVVM.ViewModels.Utility;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    [TestFixture]
    public class ObservableObjectTests
    {
        private int _callCounter;

        private void CallCounter(object sender, PropertyChangedEventArgs e)
        {
            _callCounter++;
        }

        [Test]
        public void PropertyChangedEventIsProperlySuspended()
        {
            TestViewModel vm = new TestViewModel();
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
