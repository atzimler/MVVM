namespace ATZ.MVVM.Views.Utility.Interfaces
{
    /// <summary>
    /// Modal window interface.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel of the window.</typeparam>
    public interface IModalWindow<in TViewModel> : IWindow<TViewModel>
    {
        /// <summary>
        /// Return the result of the modal view.
        /// </summary>
        bool? DialogResult { get; set; }

        /// <summary>
        /// Show the view in modal presentation.
        /// </summary>
        /// <returns>The result of the modal view.</returns>
        bool? ShowDialog();
    }
}
