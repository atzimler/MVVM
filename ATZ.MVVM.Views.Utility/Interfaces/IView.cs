namespace ATZ.MVVM.Views.Utility.Interfaces
{
    public interface IView<in TViewModel>
    {
        void BindModel(TViewModel vm);
        void UnbindModel();
    }
}
