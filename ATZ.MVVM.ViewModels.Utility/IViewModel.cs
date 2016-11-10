using System.ComponentModel;

namespace ATZ.MVVM.ViewModels.Utility
{
    public interface IViewModel<TModel> : IVerifiable, INotifyPropertyChanged
    {
        TModel Model { get; set; }
    }
}
