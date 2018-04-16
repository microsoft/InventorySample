using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Inventory.Services
{
    abstract public partial class VirtualCollection<T> : IItemsRangeInfo, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public readonly int RangeSize;

        private DispatcherTimer _timer = null;

        public VirtualCollection(int rangeSize = 16)
        {
            RangeSize = rangeSize;
            Ranges = new Dictionary<int, IList<T>>();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += OnTimerTick;
        }

        public Dictionary<int, IList<T>> Ranges { get; }

        private bool _isBusy = false;
        private bool _cancel = false;

        private IReadOnlyList<ItemIndexRange> _trackedItems = null;

        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            FetchRanges(trackedItems.Normalize().ToArray());
        }

        private object _sync = new Object();

        private void OnTimerTick(object sender, object e)
        {
            FetchRanges(_trackedItems);
        }

        private async void FetchRanges(IReadOnlyList<ItemIndexRange> trackedItems)
        {
            _trackedItems = trackedItems;

            _timer.Stop();
            lock (_sync)
            {
                if (_isBusy)
                {
                    _cancel = true;
                    _timer.Start();
                    return;
                }
                _cancel = false;
                _isBusy = true;
            }

            ClearUntrackedItems(trackedItems);
            await FetchRangesAsync(trackedItems);

            lock (_sync)
            {
                _isBusy = false;
            }
        }

        private void ClearUntrackedItems(IReadOnlyList<ItemIndexRange> trackedItems)
        {
            foreach (var rangeIndex in Ranges.Keys.ToArray())
            {
                bool isTracked = false;
                int index = rangeIndex * RangeSize;
                foreach (var trackedRange in trackedItems)
                {
                    if (trackedRange.Intersects(index, (uint)RangeSize))
                    {
                        isTracked = true;
                        break;
                    }
                }
                if (!isTracked)
                {
                    Ranges.Remove(rangeIndex);
                }
            }
        }

        private async Task FetchRangesAsync(IReadOnlyList<ItemIndexRange> trackedItems)
        {
            foreach (var trackedRange in trackedItems)
            {
                await FetchRange(trackedRange);
                if (_cancel) return;
            }
        }

        private async Task FetchRange(ItemIndexRange trackedRange)
        {
            int firstIndex = trackedRange.FirstIndex / RangeSize;
            int lastIndex = trackedRange.LastIndex / RangeSize;
            for (int index = firstIndex; index <= lastIndex; index++)
            {
                if (!Ranges.ContainsKey(index))
                {
                    var items = await FetchDataAsync(index, RangeSize);
                    Ranges[index] = items;
                    for (int n = 0; n < items.Count; n++)
                    {
                        int replaceIndex = Math.Min(index * RangeSize + n, Count);
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, items[n], null, replaceIndex));
                    }
                }
            }
        }

        virtual public void Dispose() { }

        abstract protected T DefaultItem { get; }
        abstract protected Task<IList<T>> FetchDataAsync(int pageIndex, int pageSize);
    }
}
