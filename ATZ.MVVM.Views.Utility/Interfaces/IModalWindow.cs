namespace ATZ.MVVM.Views.Utility.Interfaces
{
    public interface IModalWindow<in TViewModel> : IWindow<TViewModel>
    {
        bool? DialogResult { get; set; }
        bool? ShowDialog();
    }
}
