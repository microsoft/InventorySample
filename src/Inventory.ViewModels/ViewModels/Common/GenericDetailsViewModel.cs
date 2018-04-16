using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    abstract public partial class GenericDetailsViewModel<TModel> : ViewModelBase where TModel : ModelBase
    {
        public GenericDetailsViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        public ILookupTables LookupTables => LookupTablesProxy.Instance;

        public bool IsDataAvailable => _item != null;
        public bool IsDataUnavailable => !IsDataAvailable;

        public bool CanGoBack => !IsMainView && NavigationService.CanGoBack;

        virtual protected IEnumerable<IValidationConstraint<TModel>> ValidationConstraints => Enumerable.Empty<IValidationConstraint<TModel>>();

        private TModel _item = null;
        public TModel Item
        {
            get => _item;
            set
            {
                if (Set(ref _item, value))
                {
                    ItemReadOnly = _item;
                    IsEnabled = (!_item?.IsEmpty) ?? false;
                    NotifyPropertyChanged(nameof(IsDataAvailable));
                    NotifyPropertyChanged(nameof(IsDataUnavailable));
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }

        private TModel _itemReadOnly = null;
        public TModel ItemReadOnly
        {
            get => _itemReadOnly;
            set => Set(ref _itemReadOnly, value);
        }

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => Set(ref _isEditMode, value);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }

        public ICommand BackCommand => new RelayCommand(OnBack);
        virtual protected void OnBack()
        {
            StatusReady();
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        public ICommand EditCommand => new RelayCommand(OnEdit);
        virtual protected void OnEdit()
        {
            StatusReady();
            BeginEdit();
            MessageService.Send(this, "BeginEdit", Item);
        }
        virtual public void BeginEdit()
        {
            if (!IsEditMode)
            {
                IsEditMode = true;
                _item = _item.Clone() as TModel;
                NotifyPropertyChanged(nameof(Item));
            }
        }

        public ICommand CancelCommand => new RelayCommand(OnCancel);
        virtual protected void OnCancel()
        {
            StatusReady();
            CancelEdit();
            MessageService.Send(this, "CancelEdit", Item);
        }
        virtual public void CancelEdit()
        {
            if (IsEditMode)
            {
                Item = ItemReadOnly;
            }
            IsEditMode = false;
        }

        public ICommand SaveCommand => new RelayCommand(OnSave);
        virtual protected async void OnSave()
        {
            StatusReady();
            var result = Validate(Item);
            if (result.IsOk)
            {
                await SaveAsync();
            }
            else
            {
                await DialogService.ShowAsync(result.Message, $"{result.Description} Please, correct the error and try again.");
                StatusError($"{result.Message}: {result.Description}");
            }
        }
        virtual public async Task SaveAsync()
        {
            var original = _itemReadOnly;
            var modified = _item;
            if (modified != null)
            {
                IsEnabled = false;
                await SaveItemAsync(modified);
                IsEnabled = true;

                original?.Merge(modified);
                original?.NotifyChanges();
                Item = original;

                // TODO: Discrimine if New or Modified
                MessageService.Send(this, "ItemChanged", original);
            }
            IsEditMode = false;
        }

        public ICommand DeleteCommand => new RelayCommand(OnDelete);
        virtual protected async void OnDelete()
        {
            StatusReady();
            if (await ConfirmDeleteAsync())
            {
                await DeleteAsync();
            }
        }
        virtual public async Task DeleteAsync()
        {
            var model = Item;
            if (model != null)
            {
                IsEnabled = false;
                await DeleteItemAsync(model);

                MessageService.Send(this, "ItemDeleted", model);
            }
        }

        virtual public Result Validate(TModel model)
        {
            foreach (var constraint in ValidationConstraints)
            {
                if (!constraint.Validate(model))
                {
                    return Result.Error("Validation Error", constraint.Message);
                }
            }
            return Result.Ok();
        }

        abstract protected Task SaveItemAsync(TModel model);
        abstract protected Task DeleteItemAsync(TModel model);
        abstract protected Task<bool> ConfirmDeleteAsync();
    }
}
