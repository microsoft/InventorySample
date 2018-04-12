using System;
using System.Threading.Tasks;

using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    abstract public partial class DetailsViewModel<TModel> : ViewModelBase<TModel> where TModel : ModelBase
    {
        public event EventHandler ItemDeleted;

        public DetailsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;
            NavigationService = serviceManager.NavigationService;
            MessageService = serviceManager.MessageService;
            DialogService = serviceManager.DialogService;
            LogService = serviceManager.LogService;
        }

        public IDataProviderFactory ProviderFactory { get; }
        public INavigationService NavigationService { get; }
        public IMessageService MessageService { get; }
        public IDialogService DialogService { get; }
        public ILogService LogService { get; }

        private TModel _modelBackup = null;

        public void BeginEdit()
        {
            IsEditMode = true;
            _modelBackup = Item.Clone() as TModel;
        }

        public void CancelEdit()
        {
            if (IsEditMode)
            {
                IsEditMode = false;
                var selected = Item;
                if (selected != null)
                {
                    selected.Merge(_modelBackup);
                    selected.NotifyChanges();
                }
                _modelBackup = null;
                ItemUpdated();
            }
        }

        public async Task SaveAsync()
        {
            IsEditMode = false;
            var model = Item;
            if (model != null)
            {
                IsEnabled = false;
                await Task.Delay(100);
                await SaveItemAsync(model);
                model.NotifyChanges();
                IsEnabled = true;
            }
            _modelBackup = null;
            ItemUpdated();
        }

        public async Task DeletetAsync()
        {
            var model = Item;
            if (model != null)
            {
                IsEnabled = false;
                await Task.Delay(100);
                await DeleteItemAsync(model);
                ItemDeleted?.Invoke(this, EventArgs.Empty);
            }
            ItemUpdated();
        }

        private async Task TryExit()
        {
            if (!IsMainView)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    await NavigationService.CloseViewAsync();
                }
            }
        }

        public Result Validate()
        {
            return base.Validate(Item);
        }

        virtual protected void ItemUpdated() { }

        abstract public bool IsNewItem { get; }
        abstract protected Task SaveItemAsync(TModel model);
        abstract protected Task DeleteItemAsync(TModel model);
    }
}
