using System;
using System.ComponentModel;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    internal class TestViewModel : BaseViewModel<TestModel>
    {
        private int _propertyRaisingChangeNotification;

        public bool UnbindModelCalled { get; private set; }
        public bool UpdateValidityCalled { get; private set; }

        public int PropertyRaisingChangeNotification
        {
            get { return _propertyRaisingChangeNotification; }
            set
            {
                Set(ref _propertyRaisingChangeNotification, value);
            }
        }

        protected override void UnbindModel()
        {
            UnbindModelCalled = true;
        }

        public override void UpdateValidity(object sender, EventArgs e)
        {
            UpdateValidityCalled = true;
        }

        public override void BindModel()
        {
        }

        public new SuspendPropertyChangedEvent SuspendPropertyChangedEvent(PropertyChangedEventHandler eventHandler)
        {
            return base.SuspendPropertyChangedEvent(eventHandler);
        }

        public void SetWith2Parameters(int value)
        {
            Set(ref _propertyRaisingChangeNotification, value);
        }
    }
}
