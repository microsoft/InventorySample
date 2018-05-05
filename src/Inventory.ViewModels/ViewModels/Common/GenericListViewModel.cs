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
using System.Windows.Input;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    abstract public partial class GenericListViewModel<TModel> : ViewModelBase where TModel : ObservableObject
    {
        public GenericListViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        public ILookupTables LookupTables => LookupTablesProxy.Instance;

        public override string Title => String.IsNullOrEmpty(Query) ? $" ({ItemsCount})" : $" ({ItemsCount} for \"{Query}\")";

        private IList<TModel> _items = null;
        public IList<TModel> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        private int _itemsCount = 0;
        public int ItemsCount
        {
            get => _itemsCount;
            set => Set(ref _itemsCount, value);
        }

        private TModel _selectedItem = default(TModel);
        public TModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (Set(ref _selectedItem, value))
                {
                    if (!IsMultipleSelection)
                    {
                        MessageService.Send(this, "ItemSelected", _selectedItem);
                    }
                }
            }
        }

        private string _query = null;
        public string Query
        {
            get => _query;
            set => Set(ref _query, value);
        }

        private ListToolbarMode _toolbarMode = ListToolbarMode.Default;
        public ListToolbarMode ToolbarMode
        {
            get => _toolbarMode;
            set => Set(ref _toolbarMode, value);
        }

        private bool _isMultipleSelection = false;
        public bool IsMultipleSelection
        {
            get => _isMultipleSelection;
            set => Set(ref _isMultipleSelection, value);
        }

        public List<TModel> SelectedItems { get; protected set; }
        public IndexRange[] SelectedIndexRanges { get; protected set; }

        public ICommand NewCommand => new RelayCommand(OnNew);

        public ICommand RefreshCommand => new RelayCommand(OnRefresh);

        public ICommand StartSelectionCommand => new RelayCommand(OnStartSelection);
        virtual protected void OnStartSelection()
        {
            StatusMessage("Start selection");
            SelectedItem = null;
            SelectedItems = new List<TModel>();
            SelectedIndexRanges = null;
            IsMultipleSelection = true;
        }

        public ICommand CancelSelectionCommand => new RelayCommand(OnCancelSelection);
        virtual protected void OnCancelSelection()
        {
            StatusReady();
            SelectedItems = null;
            SelectedIndexRanges = null;
            IsMultipleSelection = false;
            SelectedItem = Items?.FirstOrDefault();
        }

        public ICommand SelectItemsCommand => new RelayCommand<IList<object>>(OnSelectItems);
        virtual protected void OnSelectItems(IList<object> items)
        {
            StatusReady();
            if (IsMultipleSelection)
            {
                SelectedItems.AddRange(items.Cast<TModel>());
                StatusMessage($"{SelectedItems.Count} items selected");
            }
        }

        public ICommand DeselectItemsCommand => new RelayCommand<IList<object>>(OnDeselectItems);
        virtual protected void OnDeselectItems(IList<object> items)
        {
            if (items?.Count > 0)
            {
                StatusReady();
            }
            if (IsMultipleSelection)
            {
                foreach (TModel item in items)
                {
                    SelectedItems.Remove(item);
                }
                StatusMessage($"{SelectedItems.Count} items selected");
            }
        }

        public ICommand SelectRangesCommand => new RelayCommand<IndexRange[]>(OnSelectRanges);
        virtual protected void OnSelectRanges(IndexRange[] indexRanges)
        {
            SelectedIndexRanges = indexRanges;
            int count = SelectedIndexRanges?.Sum(r => r.Length) ?? 0;
            StatusMessage($"{count} items selected");
        }

        public ICommand DeleteSelectionCommand => new RelayCommand(OnDeleteSelection);

        abstract protected void OnNew();
        abstract protected void OnRefresh();
        abstract protected void OnDeleteSelection();

    }
}
