using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml.Data;

namespace Inventory.Providers
{
    partial class VirtualCollection<T> : ISelectionInfo
    {
        private IList<ItemIndexRange> _rangeSelection = new List<ItemIndexRange>();

        public void SelectRange(ItemIndexRange itemIndexRange)
        {
            _rangeSelection = _rangeSelection.Merge(itemIndexRange);
        }

        public void DeselectRange(ItemIndexRange itemIndexRange)
        {
            _rangeSelection = _rangeSelection.Subtract(itemIndexRange);
        }

        public bool IsSelected(int index)
        {
            foreach (ItemIndexRange range in _rangeSelection)
            {
                if (index >= range.FirstIndex && index <= range.LastIndex) return true;
            }
            return false;
        }

        public IReadOnlyList<ItemIndexRange> GetSelectedRanges()
        {
            return _rangeSelection.ToArray();
        }
    }
}
