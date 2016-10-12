using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ATZ.MVVM.ViewModels.Utility.Connectors
{
    public class CollectionViewModelToModelConnector<TViewModel, TModel> : ObservableCollectionConnector<TModel, TViewModel>, IVerifiable
        where TViewModel : BaseViewModel<TModel>, new()
        where TModel : class
    {
        public delegate void ViewModelBinder(TViewModel vm);

        private bool _isValid;

        public ViewModelBinder BindViewModel { get; set; }
        public ViewModelBinder UnbindViewModel { get; set; }

        public bool IsValid
        {
            get { return _isValid; }
            set 
            {
                if (_isValid == value)
                {
                    return;
                }

                _isValid = value;
                OnIsValidChanged();
            }
        }

        public event EventHandler IsValidChanged;

        public ObservableCollection<TModel> ModelCollection
        {
            get { return SourceCollection; }
            set { SourceCollection = value; }
        }

        public ObservableCollection<TViewModel> ViewModelCollection
        {
            get { return TargetCollection; }
            set { TargetCollection = value; }
        }

        private void ClearViewModelCollection()
        {
            TargetCollection.ToList().ForEach(DetachViewModel);
            TargetCollection.Clear();
        }

        private void DetachViewModel(TViewModel viewModel)
        {
            viewModel.IsValidChanged -= UpdateValidity;
            UnbindViewModel?.Invoke(viewModel);
        }

        private void UpdateValidity()
        {
            IsValid = TargetCollection?.ToList().TrueForAll(vm => vm.IsValid) ?? true;
        }

        protected virtual void OnIsValidChanged()
        {
            IsValidChanged?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(IsValid));
        }

        public void ClearAllViewModelBindings()
        {
            TargetCollection?.ToList().ForEach(DetachViewModel);
        }

        public void Sort(Comparison<TModel> comparison)
        {
            bool swapped;
            do
            {
                swapped = false;
                for (var index = 0; index < SourceCollection.Count - 1; ++index)
                {
                    if (comparison(SourceCollection[index], SourceCollection[index + 1]) <= 0)
                    {
                        continue;
                    }

                    SourceCollection.Move(index + 1, index);
                    swapped = true;
                }
            } while (swapped);
        }

        public void UpdateValidity(object sender, EventArgs e)
        {
            UpdateValidity();
        }


        public override void ClearCollection() => ClearViewModelCollection();

        public override TViewModel CreateItem(TModel sourceItem)
        {
            var viewModel = new TViewModel { Model = sourceItem };
            // TODO: Replacing the viewModel.Model (above) actually executes the BindModel() below (check) and this is the only result why BindModel() is public on BaseViewModel. However,
            // the other reason could be that in this case it is similar - but results in duplicate BindModel() which could cause problems (at least performance).
            viewModel.BindModel();

            BindViewModel?.Invoke(viewModel);

            viewModel.IsValidChanged += UpdateValidity;
            return viewModel;
        }

        public override void RemoveItem(int index)
        {
            DetachViewModel(TargetCollection[index]);
            TargetCollection.RemoveAt(index);
        }

        public override void ReplaceItem(int index, TViewModel newItem)
        {
            DetachViewModel(TargetCollection[index]);
            TargetCollection[index] = newItem;
        }
    }
}
