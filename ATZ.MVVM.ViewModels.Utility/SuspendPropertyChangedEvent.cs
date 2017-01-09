using JetBrains.Annotations;
using System;

namespace ATZ.MVVM.ViewModels.Utility
{
    /// <summary>
    /// Resource locking object to suspend event signaling.
    /// </summary>
    public class SuspendPropertyChangedEvent : IDisposable
    {
        /// <summary>
        /// Delegate type for action to resume event handling.
        /// </summary>
        public delegate void ResumeEvent();

        [NotNull]
        private readonly ResumeEvent _resumeEvent;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resumeEvent">The delegate to resume event.</param>
        internal SuspendPropertyChangedEvent([NotNull] ResumeEvent resumeEvent)
        {
            _resumeEvent = resumeEvent;
        }

        /// <summary>
        /// Dispose the event suspension object.
        /// </summary>
        public void Dispose()
        {
            _resumeEvent();
        }
    }
}
