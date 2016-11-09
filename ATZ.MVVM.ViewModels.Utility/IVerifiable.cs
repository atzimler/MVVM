using System;

namespace ATZ.MVVM.ViewModels.Utility
{
    /// <summary>
    /// Provides validity verification.
    /// </summary>
    public interface IVerifiable
    {
        /// <summary>
        /// Is the object valid?
        /// </summary>
        bool IsValid { get; set; }

        /// <summary>
        /// The validity of the object has changed.
        /// </summary>
        event EventHandler IsValidChanged;

        /// <summary>
        /// Reevaluation of the object is requested.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        void UpdateValidity(object sender, EventArgs e);
    }
}
