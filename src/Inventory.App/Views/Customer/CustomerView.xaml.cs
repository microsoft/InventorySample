using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Inventory.ViewModels;
using Inventory.Services;

namespace Inventory.Views
{
    public sealed partial class CustomerView : Page
    {
        public CustomerView()
        {
            ViewModel = ServiceLocator.Current.GetService<CustomerDetailsViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            InitializeComponent();
        }

        public CustomerDetailsViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.PropertyChanged += OnPropertyChanged;
            ViewModel.ItemDeleted += OnItemDeleted;

            var state = e.Parameter as CustomerViewState;
            state = state ?? CustomerViewState.CreateDefault();
            await ViewModel.LoadAsync(state);
            UpdateTitle();

            if (state.IsNew)
            {
                await Task.Delay(100);
                details.SetFocus();
            }

            Bindings.Update();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            this.SetTitle(ViewModel.Title);
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await NavigationService.CloseViewAsync();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            ViewModel.IsEditMode = false;
            await NavigationService.CreateNewViewAsync<CustomerView>(new CustomerViewState { CustomerID = ViewModel.Item.CustomerID });
            NavigationService.GoBack();
        }
    }
}
