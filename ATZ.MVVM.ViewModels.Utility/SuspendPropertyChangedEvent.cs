using System;

namespace ATZ.MVVM.ViewModels.Utility
{
    public class SuspendPropertyChangedEvent : IDisposable
    {
        public delegate void ResumeEvent();
        private readonly ResumeEvent _resumeEvent;

        public SuspendPropertyChangedEvent(ResumeEvent resumeEvent)
        {
            _resumeEvent = resumeEvent;
        }

        public void Dispose()
        {
            _resumeEvent();
        }
    }
}
