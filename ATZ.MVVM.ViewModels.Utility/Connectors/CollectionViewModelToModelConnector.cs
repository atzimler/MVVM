using System;
using System.Collections.ObjectModel;
using System.Linq;
using ATZ.DependencyInjection;

namespace ATZ.MVVM.ViewModels.Utility.Connectors
{
    /// <summary>
    /// Connector between a collection of ViewModels and Models.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the ViewModel.</typeparam>
    /// <typeparam name="TModel">The type of the Model.</typeparam>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global => Part of public API.
    public class CollectionViewModelToModelConnector<TViewModel, TModel> : ObservableCollectionConnector<TModel, IViewModel<TModel>>, IVerifiable
        where TViewModel : IViewModel<TModel>, new()
        where TModel : class
    {
        /// <summary>
        /// Delegate function to bind a ViewModel.
        /// </summary>
        /// <param name="vm">The ViewModel to bind.</param>
        public delegate void ViewModelBinder(IViewModel<TModel> vm);

        private bool _isValid;

        /// <summary>
        /// Delegate to bind the ViewModel.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global => Part of public API.
        public ViewModelBinder BindViewModel { get; set; }

        /// <summary>
        /// Delegate to unbind the ViewModel.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global => Part of public API.
        public ViewModelBinder UnbindViewModel { get; set; }

        /// <summary>
        /// The validity of the object.
        /// </summary>
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

        /// <summary>
        /// The validity of the object has changed.
        /// </summary>
        public event EventHandler IsValidChanged;

        /// <summary>
        /// The collection of the Model objects.
        /// </summary>
        public ObservableCollection<TModel> ModelCollection
        {
            get { return SourceCollection; }
            set { SourceCollection = value; }
        }

        /// <summary>
        /// The collection of the ViewModel objects.
        /// </summary>
        public ObservableCollection<IViewModel<TModel>> ViewModelCollection
        {
            get { return TargetCollection; }
            set { TargetCollection = value; }
        }

        private void ClearViewModelCollection()
        {
            TargetCollection.ToList().ForEach(DetachViewModel);
            TargetCollection.Clear();
        }

        private void DetachViewModel(IViewModel<TModel> viewModel)
        {
            viewModel.IsValidChanged -= UpdateValidity;
            UnbindViewModel?.Invoke(viewModel);
        }

        private void UpdateValidity()
        {
            IsValid = TargetCollection?.ToList().TrueForAll(vm => vm.IsValid) ?? true;
        }

        /// <summary>
        /// Fire IsValidChanged event and property change notification for IsValid property.
        /// </summary>
        protected virtual void OnIsValidChanged()
        {
            IsValidChanged?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(IsValid));
        }

        /// <summary>
        /// Detach all ViewModels in the mirror collection.
        /// </summary>
        public void ClearAllViewModelBindings()
        {
            TargetCollection?.ToList().ForEach(DetachViewModel);
        }

        /// <summary>
        /// Sort the Model collection with the given comparison method.
        /// </summary>
        /// <param name="comparison">The comparison method to compare the Model objects.</param>
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

        /// <summary>
        /// Reevaluation of the object validity requested.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        public void UpdateValidity(object sender, EventArgs e)
        {
            UpdateValidity();
        }

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ClearCollection"/>
        public override void ClearCollection() => ClearViewModelCollection();

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.CreateItem"/>
        public override IViewModel<TModel> CreateItem(TModel sourceItem)
        {
            var viewModel = DependencyResolver.Instance.GetInterface<IViewModel<TModel>>(
                typeof(IViewModel<>), sourceItem.GetType());
            viewModel.Model = sourceItem;

            BindViewModel?.Invoke(viewModel);

            viewModel.IsValidChanged += UpdateValidity;
            return viewModel;
        }

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.RemoveItem"/>
        public override void RemoveItem(int index)
        {
            DetachViewModel(TargetCollection[index]);
            TargetCollection.RemoveAt(index);
        }

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ReplaceItem"/>
        public override void ReplaceItem(int index, IViewModel<TModel> newItem)
        {
            DetachViewModel(TargetCollection[index]);
            TargetCollection[index] = newItem;
        }
    }
}
