namespace ATZ.MVVM.Views.Utility.Interfaces
{
    public interface IView<VM>
    {
        void BindModel(VM vm);
        void UnbindModel();
    }
}
