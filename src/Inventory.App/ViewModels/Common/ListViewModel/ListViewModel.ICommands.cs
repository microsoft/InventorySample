using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.ViewModels
{
    partial class ListViewModel<TModel>
    {
        public ICommand NewCommand => new RelayCommand(New);
        abstract public void New();

        public ICommand RefreshCommand => new RelayCommand(Refresh);
        virtual protected async void Refresh()
        {
            await RefreshAsync();
        }

        public List<TModel> _selectedItems = null;

        public ICommand StartSelectionCommand => new RelayCommand(StartSelection);
        virtual protected void StartSelection()
        {
            _selectedItems = new List<TModel>();
            _selectedIndexRanges = null;
            IsMultipleSelection = true;
        }

        public ICommand CancelSelectionCommand => new RelayCommand(CancelSelection);
        virtual protected void CancelSelection()
        {
            _selectedItems?.Clear();
            _selectedItems = null;
            _selectedIndexRanges = null;
            IsMultipleSelection = false;
            SelectedItem = Items?.FirstOrDefault();
        }

        public ICommand SelectItemsCommand => new RelayCommand<IList<object>>(SelectItems);
        virtual protected void SelectItems(IList<object> items)
        {
            if (IsMultipleSelection)
            {
                foreach (TModel item in items)
                {
                    _selectedItems.Add(item);
                }
            }
        }

        public ICommand DeselectItemsCommand => new RelayCommand<IList<object>>(DeselectItems);
        virtual protected void DeselectItems(IList<object> items)
        {
            if (IsMultipleSelection)
            {
                foreach (TModel item in items)
                {
                    _selectedItems.Remove(item);
                }
            }
        }

        private IndexRange[] _selectedIndexRanges = null;

        public ICommand SelectRangesCommand => new RelayCommand<IndexRange[]>(SelectRanges);
        private void SelectRanges(IndexRange[] indexRanges)
        {
            _selectedIndexRanges = indexRanges;
        }

        public ICommand DeleteSelectionCommand => new RelayCommand(DeleteSelection);
        virtual protected async void DeleteSelection()
        {
            if (await ConfirmDeleteSelectionAsync())
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    if (_selectedIndexRanges != null)
                    {
                        await DeleteRangesAsync(dataProvider, _selectedIndexRanges);
                        await RefreshAsync(dataProvider);
                        _selectedIndexRanges = null;
                    }
                    else if (_selectedItems != null)
                    {
                        await DeleteItemsAsync(dataProvider, _selectedItems);
                        await RefreshAsync(dataProvider);
                        _selectedItems.Clear();
                        _selectedItems = null;
                    }
                }
                //IsMultipleSelection = false;
            }
        }

        abstract protected Task<bool> ConfirmDeleteSelectionAsync();
    }
}
