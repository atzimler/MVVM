using System.ComponentModel;

namespace ATZ.MVVM.ViewModels.Utility
{
    /// <summary>
    /// ViewModel interface to provide flexible types because multiple inheritation is not supported by the .NET Framework.
    /// </summary>
    /// <typeparam name="TModel">The type of the Model associated with the ViewModel.</typeparam>
    public interface IViewModel<TModel> : IVerifiable, INotifyPropertyChanged
    {
        /// <summary>
        /// Get the value of the Model property in covariant way.
        /// </summary>
        /// <returns>The value of the Model property.</returns>
        TModel GetModel();

        /// <summary>
        /// Set the value of the Model property in contravariant way.
        /// </summary>
        /// <param name="model">The new value of the Model property.</param>
        void SetModel(TModel model);
    }
}
