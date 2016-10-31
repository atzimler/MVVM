using System.Windows;

namespace ATZ.MVVM.Views.Utility.Interfaces
{
    public interface IView<in TViewModel>
    {
        // ReSharper disable once InconsistentNaming => Name of the type is spelled correctly as UIElement
        UIElement UIElement { get; }

        void BindModel(TViewModel vm);
        void UnbindModel();
    }
}
