using System;
using System.Collections.Generic;

using Inventory.Controls;

namespace Inventory.ViewModels
{
    partial class ListViewModel<TModel>
    {
        public override string Title => String.IsNullOrEmpty(Query) ? " " : $"results for \"{Query}\"";

        public bool IsDataAvailable => (Items?.Count ?? 0) > 0;

        public string Query { get; set; }

        private ListToolbarMode _toolbarMode = ListToolbarMode.Default;
        public ListToolbarMode ToolbarMode
        {
            get => _toolbarMode;
            set => Set(ref _toolbarMode, value);
        }

        private IList<TModel> _items = null;
        public IList<TModel> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        private bool _isMultipleSelection = false;
        public bool IsMultipleSelection
        {
            get => _isMultipleSelection;
            set => Set(ref _isMultipleSelection, value);
        }

        private TModel _selectedItem = default(TModel);
        public TModel SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private int _itemsCount = 0;
        public int ItemsCount
        {
            get => _itemsCount;
            set => Set(ref _itemsCount, value);
        }
    }
}
