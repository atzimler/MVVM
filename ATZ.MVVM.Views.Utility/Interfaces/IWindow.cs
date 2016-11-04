using System.Windows;

namespace ATZ.MVVM.Views.Utility.Interfaces
{
    /// <summary>
    /// Window interface
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel of the window.</typeparam>
    public interface IWindow<in TViewModel> : IView<TViewModel>
    {
        /// <see cref="Window.Close"/>
        void Close();
    }
}
