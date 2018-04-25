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

        private void ModoAutoClick(object sender, RoutedEventArgs e)
        {
            labelTB01.Mode = Controls.TextEditMode.Auto;
            labelTB02.Mode = Controls.TextEditMode.Auto;
        }

        private void ModoReadOnly(object sender, RoutedEventArgs e)
        {
            labelTB01.Mode = Controls.TextEditMode.ReadOnly;
            labelTB02.Mode = Controls.TextEditMode.ReadOnly;
        }

        private void ModeWrite(object sender, RoutedEventArgs e)
        {
            labelTB01.Mode = Controls.TextEditMode.ReadWrite;
            labelTB02.Mode = Controls.TextEditMode.ReadWrite;
        }
    }
}
