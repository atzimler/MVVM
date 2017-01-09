using ATZ.DependencyInjection;
using JetBrains.Annotations;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ATZ.MVVM.ViewModels.Utility.Connectors
{
    /// <summary>
    /// Connector between a collection of ViewModels and Models.
    /// </summary>
    /// <typeparam name="TModel">The type of the Model.</typeparam>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global => Part of public API.
    public class CollectionViewModelToModelConnector<TModel> : ObservableCollectionConnector<TModel, IViewModel<TModel>>, IVerifiable
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
            if (TargetCollection == null)
            {
                return;
            }

            TargetCollection.ToList().ForEach(DetachViewModel);
            TargetCollection.Clear();
        }

        private void DetachViewModel(IViewModel<TModel> viewModel)
        {
            if (viewModel == null)
            {
                return;
            }

            viewModel.IsValidChanged -= UpdateValidity;
            UnbindViewModel?.Invoke(viewModel);
        }

        private static void SortCollection([NotNull] ObservableCollection<TModel> collection, [NotNull] Comparison<TModel> comparison)
        {
            bool swapped;
            do
            {
                swapped = false;
                for (var index = 0; index < collection.Count - 1; ++index)
                {
                    if (comparison(collection[index], collection[index + 1]) <= 0)
                    {
                        continue;
                    }

                    collection.Move(index + 1, index);
                    swapped = true;
                }
            } while (swapped);
        }

        private void UpdateValidity()
        {
            IsValid = TargetCollection?.ToList().TrueForAll(ViewModelIsNullOrValid) ?? true;
        }

        private static bool ViewModelIsNullOrValid(IViewModel<TModel> vm)
        {
            return vm == null || vm.IsValid;
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
        /// Add the ViewModel to the ViewModel collection and at the same time add the ViewModel's Model to the Model collection.
        /// </summary>
        /// <param name="viewModel">The ViewModel to add to the ViewModel collection.</param>
        /// <exception cref="ArgumentNullException">viewModel is null.</exception>
        public void Add(IViewModel<TModel> viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            Add(viewModel.GetModel(), viewModel);
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
        /// <exception cref="ArgumentNullException">comparison is null.</exception>
        public void Sort(Comparison<TModel> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }

            var collectionSorted = SourceCollection;
            if (collectionSorted == null)
            {
                return;
            }

            SortCollection(collectionSorted, comparison);
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
            if (viewModel == null)
            {
                return null;
            }

            viewModel.SetModel(sourceItem);

            BindViewModel?.Invoke(viewModel);

            viewModel.IsValidChanged += UpdateValidity;
            return viewModel;
        }

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.RemoveItem"/>
        public override void RemoveItem(int index)
        {
            var collection = TargetCollection;
            if (collection == null)
            {
                return;
            }

            var viewModel = collection[index];

            DetachViewModel(viewModel);
            collection.RemoveAt(index);
        }

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ReplaceItem"/>
        public override void ReplaceItem(int index, IViewModel<TModel> newItem)
        {
            var collection = TargetCollection;
            if (collection == null)
            {
                return;
            }

            var viewModel = collection[index];

            DetachViewModel(viewModel);
            collection[index] = newItem;
        }
    }
}
