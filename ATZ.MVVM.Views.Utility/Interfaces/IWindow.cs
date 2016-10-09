namespace ATZ.MVVM.Views.Utility.Interfaces
{
    public interface IWindow<in TViewModel> : IView<TViewModel>
    {
        void Close();
    }
}
