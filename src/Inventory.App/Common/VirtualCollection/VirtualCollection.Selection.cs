#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml.Data;

namespace Inventory.Services
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
