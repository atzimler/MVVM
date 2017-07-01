namespace ATZ.MVVM.Views.Utility.Interfaces
{
    /// <summary>
    /// Window interface
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel of the window.</typeparam>
    public interface IWindow<in TViewModel> : IView<TViewModel>
    {
        /// <summary>
        /// Close the view. Depending on the type of the view or window, this could mean hiding or closing in case of message boxes.
        /// </summary>
        void Close();
    }
}
