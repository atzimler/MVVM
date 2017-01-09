using ATZ.MVVM.ViewModels.Utility;
using ATZ.MVVM.ViewModels.Utility.Connectors;
using JetBrains.Annotations;
using System;
using System.Collections.ObjectModel;

namespace ATZ.MVVM.Views.Utility.Connectors
{
    /// <summary>
    /// Mirror an ObservableCollection into another ObservableCollection with a transformation of the source object to the mirror object.
    /// </summary>
    /// <typeparam name="TSource">The type of the items in the source collection.</typeparam>
    /// <typeparam name="TTarget">The type of the items in the mirror collection.</typeparam>
    public class ObservableCollectionCopierConnector<TSource, TTarget> : ObservableCollectionConnector<TSource, TTarget>
    {
        /// <summary>
        /// The source collection.
        /// </summary>
        public new ObservableCollection<TSource> SourceCollection
        {
            get { return base.SourceCollection; }
            set { base.SourceCollection = value; }
        }

        /// <summary>
        /// The mirror collection.
        /// </summary>
        public new ObservableCollection<TTarget> TargetCollection
        {
            get { return base.TargetCollection; }
            set { base.TargetCollection = value; }
        }

        [NotNull]
        private readonly Func<TSource, TTarget> _transformSourceToTarget;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transformSourceToTarget">The transformation from the source item to the mirror item.</param>
        public ObservableCollectionCopierConnector([NotNull] Func<TSource, TTarget> transformSourceToTarget)
        {
            _transformSourceToTarget = transformSourceToTarget;
        }

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ClearCollection"/>
        public override void ClearCollection() => TargetCollection?.Clear();

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.CreateItem"/>
        public override TTarget CreateItem(TSource sourceItem) => _transformSourceToTarget(sourceItem);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.RemoveItem"/>
        public override void RemoveItem(int index) => TargetCollection?.RemoveAt(index);

        /// <see cref="ICollectionChangedEventSource{TSourceItem,TCollectionItem}.ReplaceItem"/>
        public override void ReplaceItem(int index, TTarget newItem)
        {
            var collection = TargetCollection;
            if (collection == null)
            {
                return;
            }

            collection[index] = newItem;
        }
    }
}
