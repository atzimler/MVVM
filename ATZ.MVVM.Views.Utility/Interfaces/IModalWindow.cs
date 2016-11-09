using System.Windows;

namespace ATZ.MVVM.Views.Utility.Interfaces
{
    /// <summary>
    /// Modal window interface.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel of the window.</typeparam>
    public interface IModalWindow<in TViewModel> : IWindow<TViewModel>
    {
        /// <see cref="Window.DialogResult"/>
        bool? DialogResult { get; set; }

        /// <see cref="Window.ShowDialog"/>
        bool? ShowDialog();
    }
}
