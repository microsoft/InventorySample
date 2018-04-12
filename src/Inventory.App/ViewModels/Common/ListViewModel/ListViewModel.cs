using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    abstract public partial class ListViewModel<TModel> : ViewModelBase where TModel : ModelBase
    {
        public ListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
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

        public async Task RefreshAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await RefreshAsync(dataProvider);
                SelectedItem = Items.FirstOrDefault();
            }
        }

        virtual protected async Task RefreshAsync(IDataProvider dataProvider)
        {
            Items = null;
            SelectedItem = null;

            Items = await GetItemsAsync(dataProvider);
            ItemsCount = Items.Count;

            NotifyPropertyChanged(nameof(Title));
            NotifyPropertyChanged(nameof(IsDataAvailable));
        }

        virtual public void ApplyViewState(ListViewState state)
        {
            Query = state.Query;
        }

        virtual public void UpdateViewState(ListViewState state)
        {
            state.Query = Query;
        }

        abstract public Task<IList<TModel>> GetItemsAsync(IDataProvider dataProvider);
        abstract protected Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<TModel> models);
        abstract protected Task DeleteRangesAsync(IDataProvider dataProvider, IEnumerable<IndexRange> ranges);
    }
}
