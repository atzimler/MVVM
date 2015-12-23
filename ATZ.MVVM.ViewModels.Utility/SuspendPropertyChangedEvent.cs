using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATZ.MVVM.ViewModels.Utility
{
    public class SuspendPropertyChangedEvent : IDisposable
    {
        public delegate void ResumeEvent();
        private ResumeEvent _resumeEvent;

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
