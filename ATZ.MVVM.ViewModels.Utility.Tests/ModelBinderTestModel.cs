using System.Collections.ObjectModel;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    public class ModelBinderTestModel
    {
        public ObservableCollection<TestModel> Entries { get; } = new ObservableCollection<TestModel>();
    }
}