using System;
using System.ComponentModel;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    public class TestViewModel : BaseViewModel<TestModel>
    {
        private int _a;
        private int _propertyRaisingChangeNotification;

        public bool ExecuteCalled { get; private set; }
        public bool UnbindModelCalled { get; private set; }
        public bool UpdateValidityCalled { get; private set; }

        public int A
        {
            get { return _a; }
            set { Set(ref _a, value, new[] {"B"}); }
        }

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

        public void NullAdditionalProperties()
        {
            Set(ref _propertyRaisingChangeNotification, _propertyRaisingChangeNotification + 1, null);
        }

        public void SetWith2Parameters(int value)
        {
            Set(ref _propertyRaisingChangeNotification, value);
        }

        public new SuspendPropertyChangedEvent SuspendPropertyChangedEvent(PropertyChangedEventHandler eventHandler)
        {
            return base.SuspendPropertyChangedEvent(eventHandler);
        }

        public void Execute()
        {
            ExecuteCalled = true;
        }
    }
}
