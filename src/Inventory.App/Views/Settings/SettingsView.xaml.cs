using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class SettingsView : Page
    {
        public SettingsView()
        {
            ViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();
            InitializeComponent();
        }

        public SettingsViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadAsync(e.Parameter as SettingsArgs);
        }
    }
}
