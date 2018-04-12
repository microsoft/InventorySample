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
    public sealed partial class OrderItemView : Page
    {
        public OrderItemView()
        {
            ViewModel = ServiceLocator.Current.GetService<OrderItemDetailsViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            InitializeComponent();
        }

        public OrderItemDetailsViewModel ViewModel { get; }
        public INavigationService NavigationService { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.PropertyChanged += OnPropertyChanged;
            ViewModel.ItemDeleted += OnItemDeleted;

            var state = e.Parameter as OrderItemViewState;
            state = state ?? OrderItemViewState.CreateDefault();
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
            await NavigationService.CreateNewViewAsync<OrderItemView>(new OrderItemViewState(ViewModel.Item.OrderID) { OrderLine = ViewModel.Item.OrderLine });
            NavigationService.GoBack();
        }
    }
}
