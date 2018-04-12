using System;
using System.Threading.Tasks;

using Inventory.Services;
using Inventory.Providers;


namespace Inventory.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(IDataProviderFactory providerFactory, INavigationService navigationService)
        {
            ProviderFactory = providerFactory;
            NavigationService = navigationService;
        }

        public IDataProviderFactory ProviderFactory { get; }
        public INavigationService NavigationService { get; }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        private string _statusMessage = null;
        public string StatusMessage
        {
            get => _statusMessage;
            set => Set(ref _statusMessage, value);
        }

        public ShellViewState ViewState { get; protected set; }

        virtual public async Task LoadAsync(ShellViewState viewState)
        {
            // TODOX: Implement as a Scoped service?
            await DataHelper.Current.InitializeAsync(ProviderFactory);

            ViewState = viewState ?? new ShellViewState();
            NavigationService.Navigate(ViewState.ViewModel, ViewState.Parameter);
        }

        virtual public void Unload()
        {
        }
    }
}
